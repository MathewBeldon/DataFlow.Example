using DataFlow.Example.Services;

namespace DataFlow.Example.Features
{
    public sealed class SemaphoreProcess
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IFakeTransformer _fakeTransformer;

        public SemaphoreProcess(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer)
        {
            _fakeRepository = fakeRepository;
            _fakeTransformer = fakeTransformer;
        }

        public async Task<bool> ProcessAsync(int amount, CancellationToken cancellationToken)
        {
            using (var semaphore = new SemaphoreSlim(Environment.ProcessorCount))
            {
                var data = await _fakeRepository.GetDataAsync(amount, cancellationToken);
                var tasks = new List<Task>();
                foreach (var item in data)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            await semaphore.WaitAsync();
                            var transformedData = await _fakeTransformer.TransformDataAsync(item, cancellationToken);
                            foreach (var saveItem in transformedData)
                            {
                                await _fakeRepository.SaveDataAsync(saveItem, cancellationToken);
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }
                await Task.WhenAll(tasks);
                return true;
            }
        }
    }
}
