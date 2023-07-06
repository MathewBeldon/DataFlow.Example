namespace DataFlow.Example.Services
{
    public sealed class FakeRepository : IFakeRepository
    {
        public async Task<IEnumerable<int>> GetDataAsync(int amountOfItemsToReturn, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return Enumerable.Range(0, amountOfItemsToReturn);
        }

        public async Task<string> SaveDataAsync(string itemToSave, CancellationToken cancellationToken)
        {
            await Task.Delay(200);
            var itemIdToReturn = Guid.NewGuid().ToString();
            return itemIdToReturn;
        }
    }
}
