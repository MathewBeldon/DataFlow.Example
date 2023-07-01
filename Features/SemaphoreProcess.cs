using DataFlow.Example.Services;
using System.Collections.Concurrent;

namespace DataFlow.Example.Features
{
    public sealed class SemaphoreProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;
        private readonly IFakeTelemetry _fakeTelemetry;

        public SemaphoreProcess(
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
            using (var semaphore = new SemaphoreSlim(Environment.ProcessorCount))
            {
                var data = await _fakeRepository.GetDataAsync(amount, cancellationToken);
                var tasks = new List<Task<List<string>>>();
                var primaryKeys = new ConcurrentBag<string>();
                foreach (var item in data)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        IEnumerable<string> transformedData;
                        try
                        {
                            await semaphore.WaitAsync();
                            transformedData = await _fakeTransformer.TransformDataAsync(item, cancellationToken);
                            
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                        var result = await Task.WhenAll(transformedData.Select(saveItem => _fakeRepository.SaveDataAsync(saveItem, cancellationToken)));
                        _ = Task.Run(() => Task.WhenAll(result.Select(postItem => _fakeTelemetry.PostTelemetry(postItem, cancellationToken))));
                        return result.ToList();
                    }));
                }
                var results = await Task.WhenAll(tasks);
                return results.SelectMany(res => res);
            }
        }
    }
}
