namespace DataFlow.Example.Services
{
    public sealed class FakeRepository : IFakeRepository
    {
        public Task<IEnumerable<int>> GetDataAsync(int amount, CancellationToken cancellationToken)
        {
            Thread.Sleep(20);
            return Task.FromResult(Enumerable.Range(0, amount));
        }

        public Task SaveDataAsync(string item, CancellationToken cancellationToken)
        {
            Thread.Sleep(100);
            return Task.FromResult(true);
        }
    }
}
