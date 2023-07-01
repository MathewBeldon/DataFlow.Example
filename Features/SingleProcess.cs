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

        public async Task<IEnumerable<string>> ProcessAsync(int amount, CancellationToken cancellationToken)
        {
            var primaryKeys = new List<string>();
            var data = await _fakeRepository.GetDataAsync(amount, cancellationToken);
            foreach (var item in data)
            {
                var transformedData = await _fakeTransformer.TransformDataAsync(item, cancellationToken);
                foreach (var saveItem in transformedData)
                {
                    primaryKeys.Add(await _fakeRepository.SaveDataAsync(saveItem, cancellationToken));
                }
            }

            return primaryKeys;
        }
    }
}
