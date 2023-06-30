namespace DataFlow.Example.Services
{
    public interface IFakeRepository
    {
        Task<IEnumerable<int>> GetDataAsync(int amount, CancellationToken cancellationToken);
        Task SaveDataAsync(string item, CancellationToken cancellationToken);
    }
}
