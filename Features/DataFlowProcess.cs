using DataFlow.Example.Services;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;

namespace DataFlow.Example.Features
{
    public sealed class DataFlowProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;
        private readonly IFakeTelemetry _fakeTelemetry;

        private readonly ExecutionDataflowBlockOptions _dataflowOptionsLimited;
        private readonly ExecutionDataflowBlockOptions _dataflowOptionsUnlimited;

        private readonly DataflowLinkOptions _dataflowLinkOptions;

        public DataFlowProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer,
            IFakeTelemetry fakeTelemetry)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;
            _fakeTelemetry = fakeTelemetry;

            _dataflowOptionsLimited = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            _dataflowOptionsUnlimited = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            };

            _dataflowLinkOptions = new DataflowLinkOptions() { PropagateCompletion = true };
        }

        public async IAsyncEnumerable<string> ProcessAsync(int amount, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var getDataBlock = new TransformManyBlock<int, int>((amount) => _fakeRepository.GetDataAsync(amount, cancellationToken), _dataflowOptionsUnlimited);
            var transformDataBlock = new TransformManyBlock<int, string>((amount) => _fakeTransformer.TransformDataAsync(amount, cancellationToken), _dataflowOptionsLimited);
            var saveDataBlock = new TransformBlock<string, string>((input) => _fakeRepository.SaveDataAsync(input, cancellationToken), _dataflowOptionsUnlimited);
            var broadcastBlock = new BroadcastBlock<string>(x => x);
            var returnBufferBlock = new BufferBlock<string>();
            var postTelemetryBlock = new ActionBlock<string>((input) => _fakeTelemetry.PostTelemetryAsync(input, cancellationToken), _dataflowOptionsUnlimited);

            getDataBlock.LinkTo(transformDataBlock, _dataflowLinkOptions);
            transformDataBlock.LinkTo(saveDataBlock, _dataflowLinkOptions);
            saveDataBlock.LinkTo(broadcastBlock, _dataflowLinkOptions);
            broadcastBlock.LinkTo(returnBufferBlock, _dataflowLinkOptions);
            broadcastBlock.LinkTo(postTelemetryBlock);

            await getDataBlock.SendAsync(amount, cancellationToken);
            getDataBlock.Complete();

            await saveDataBlock.Completion;

            while (await returnBufferBlock.OutputAvailableAsync())
            {
                while (returnBufferBlock.TryReceive(out var item))
                {
                    yield return item;
                }
            }
        }
    }
}
