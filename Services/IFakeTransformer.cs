namespace DataFlow.Example.Services
{
    public interface IFakeTransformer
    {
        Task<IEnumerable<string>> TransformDataAsync(int itemNumberToTransform, CancellationToken cancellationToken);
    }
}
