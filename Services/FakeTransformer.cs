namespace DataFlow.Example.Services
{
    public sealed class FakeTransformer : IFakeTransformer
    {
        public Task<IEnumerable<string>> TransformDataAsync(int itemNumberToTransform, CancellationToken cancellationToken)
        {
            var listOfStrings = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                listOfStrings.Add(itemNumberToTransform.ToString());
            }

            Thread.Sleep(30);
            return Task.FromResult((IEnumerable<string>)listOfStrings);
        }
    }
}
