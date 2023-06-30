namespace DataFlow.Example.Services
{
    public sealed class FakeTransformer : IFakeTransformer
    {
        public async Task<IEnumerable<string>> TransformDataAsync(int number, CancellationToken cancellationToken)
        {
            var listOfStrings = new List<string>();
            for (int i = 0; i < number; i++)
            {
                listOfStrings.Add("strings");
            }

            Thread.Sleep(300);
            return listOfStrings;
        }
    }
}
