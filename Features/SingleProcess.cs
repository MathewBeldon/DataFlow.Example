using DataFlow.Example.Services;

namespace DataFlow.Example.Features
{
    public sealed class SingleProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;
        private readonly IFakeTelemetry _fakeTelemetry;

        public SingleProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer,
            IFakeTelemetry fakeTelemetry)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;
            _fakeTelemetry = fakeTelemetry;
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
                    var result = await _fakeRepository.SaveDataAsync(saveItem, cancellationToken);
                    primaryKeys.Add(result);
                    _ = Task.Run(() => _fakeTelemetry.PostTelemetryAsync(result, cancellationToken));
                }
            }

            return primaryKeys;
        }
    }
}
