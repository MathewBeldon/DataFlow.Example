namespace DataFlow.Example.Services
{
    public sealed class FakeRepository : IFakeRepository
    {
        public async Task<IEnumerable<int>> GetDataAsync(int amount, CancellationToken cancellationToken)
        {
            await Task.Delay(20);
            return Enumerable.Range(0, amount);
        }

        public async Task SaveDataAsync(string item, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
        }
    }
}
