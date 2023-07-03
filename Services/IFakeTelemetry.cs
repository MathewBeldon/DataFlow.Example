namespace DataFlow.Example.Services
{
    public interface IFakeTelemetry
    {
        Task PostTelemetryAsync(string input, CancellationToken cancellationToken);
    }
}
