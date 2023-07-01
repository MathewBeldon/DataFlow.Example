namespace DataFlow.Example.Services
{
    public sealed class FakeTransformer : IFakeTransformer
    {
        public Task<IEnumerable<string>> TransformDataAsync(int number, CancellationToken cancellationToken)
        {
            var listOfStrings = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                listOfStrings.Add("strings");
            }

            Thread.Sleep(30);
            return Task.FromResult((IEnumerable<string>)listOfStrings);
        }
    }
}
