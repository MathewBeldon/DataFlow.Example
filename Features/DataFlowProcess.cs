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

        private readonly DataflowLinkOptions _dateflowLinkOptions;

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

            _dateflowLinkOptions = new DataflowLinkOptions() { PropagateCompletion = true };
        }

        public async IAsyncEnumerable<string> ProcessAsync(int amount, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var getDataBlock = new TransformManyBlock<int, int>((amount) => _fakeRepository.GetDataAsync(amount, default), _dataflowOptionsUnlimited);
            var transformDataBlock = new TransformManyBlock<int, string>((amount) => _fakeTransformer.TransformDataAsync(amount, default), _dataflowOptionsLimited);
            var saveDataBlock = new TransformBlock<string, string>((input) => _fakeRepository.SaveDataAsync(input, default), _dataflowOptionsUnlimited);
            var returnBufferBlock = new BufferBlock<string>();
            var postTelemetryBlock = new ActionBlock<string>((input) => _fakeTelemetry.PostTelemetry(input, default), _dataflowOptionsUnlimited);

            getDataBlock.LinkTo(transformDataBlock, _dateflowLinkOptions);
            transformDataBlock.LinkTo(saveDataBlock, _dateflowLinkOptions);
            saveDataBlock.LinkTo(returnBufferBlock, _dateflowLinkOptions);
            saveDataBlock.LinkTo(postTelemetryBlock, _dateflowLinkOptions);

            await getDataBlock.SendAsync(amount, cancellationToken);

            getDataBlock.Complete();

            await saveDataBlock.Completion;

            while (returnBufferBlock.TryReceive(out var item))
            {
                yield return item;
            }
        }
    }
}
