using DataFlow.Example.Services;

namespace DataFlow.Example.Features
{
    public sealed class SingleProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;

        public SingleProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;
        }

        public async Task<bool> ProcessAsync(int amount, CancellationToken cancellationToken)
        {
            var data = await _fakeRepository.GetDataAsync(amount, cancellationToken);
            foreach (var item in data)
            {
                var transformedData = await _fakeTransformer.TransformDataAsync(item, cancellationToken);
                foreach (var saveItem in transformedData)
                {
                    await _fakeRepository.SaveDataAsync(saveItem, cancellationToken);
                }
            }

            return true;
        }
    }
}
