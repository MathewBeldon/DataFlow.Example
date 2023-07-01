namespace DataFlow.Example.Services
{
    public sealed class FakeTelemetry : IFakeTelemetry
    {
        public async Task PostTelemetry(string input, CancellationToken cancellationToken)
        {
            await Task.Delay(50);
        }
    }
}
