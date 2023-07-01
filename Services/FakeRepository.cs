namespace DataFlow.Example.Services
{
    public sealed class FakeRepository : IFakeRepository
    {
        public async Task<IEnumerable<int>> GetDataAsync(int amount, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return Enumerable.Range(0, amount);
        }

        public async Task<string> SaveDataAsync(string item, CancellationToken cancellationToken)
        {
            await Task.Delay(200);
            return Guid.NewGuid().ToString();
        }
    }
}
