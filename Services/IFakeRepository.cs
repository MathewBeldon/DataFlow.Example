namespace DataFlow.Example.Services
{
    public interface IFakeRepository
    {
        Task<IEnumerable<int>> GetDataAsync(int amount, CancellationToken cancellationToken);
        Task<string> SaveDataAsync(string item, CancellationToken cancellationToken);
    }
}
