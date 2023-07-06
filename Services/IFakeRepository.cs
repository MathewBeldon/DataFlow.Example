namespace DataFlow.Example.Services
{
    public interface IFakeRepository
    {
        Task<IEnumerable<int>> GetDataAsync(int amountOfItemsToReturn, CancellationToken cancellationToken);
        Task<string> SaveDataAsync(string itemToSave, CancellationToken cancellationToken);
    }
}
