using DataFlow.Example.Services;
using System.Threading.Tasks.Dataflow;

namespace DataFlow.Example.Features
{
    public sealed class DataFlowProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;

        private readonly TransformManyBlock<int, int> _getDataBlock;
        private readonly TransformManyBlock<int, string> _transformDataBlock;
        private readonly ActionBlock<string> _saveDataBlock;

        public DataFlowProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            _getDataBlock = new TransformManyBlock<int, int>((amount) => _fakeRepository.GetDataAsync(amount, default), options);
            _transformDataBlock = new TransformManyBlock<int, string>((amount) => _fakeTransformer.TransformDataAsync(amount, default), options);
            _saveDataBlock = new ActionBlock<string>((input) => _fakeRepository.SaveDataAsync(input, default), options);

            DataflowLinkOptions linkOptions = new DataflowLinkOptions() { PropagateCompletion = true };

            _getDataBlock.LinkTo(_transformDataBlock, linkOptions);
            _transformDataBlock.LinkTo(_saveDataBlock, linkOptions);
        }

        public async Task<bool> ProcessAsync(int amount, CancellationToken cancellationToken)
        {
            await _getDataBlock.SendAsync(amount, cancellationToken);

            _getDataBlock.Complete();

            await _saveDataBlock.Completion;

            return true;
        }
    }
}
