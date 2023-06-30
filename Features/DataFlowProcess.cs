using DataFlow.Example.Services;
using System.Threading.Tasks.Dataflow;

namespace DataFlow.Example.Features
{
    public sealed class DataFlowProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;

        public DataFlowProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;
        }

        public async Task<bool> ProcessAsync(int amount, CancellationToken cancellationToken)
        {
            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
            };

            var getDataBlock = new TransformManyBlock<int, int>((amount) => _fakeRepository.GetDataAsync(amount, cancellationToken), options);
            var transformDataBlock = new TransformManyBlock<int, string>((amount) => _fakeTransformer.TransformDataAsync(amount, cancellationToken), options);
            var saveDataBlock = new ActionBlock<string>((input) => _fakeRepository.SaveDataAsync(input, cancellationToken), options);

            DataflowLinkOptions linkOptions = new DataflowLinkOptions() {  PropagateCompletion = true };

            getDataBlock.LinkTo(transformDataBlock, linkOptions);
            transformDataBlock.LinkTo(saveDataBlock, linkOptions);

            await getDataBlock.SendAsync(amount, cancellationToken);

            getDataBlock.Complete();

            await saveDataBlock.Completion;

            return await Task.FromResult(true);
        }
    }
}
