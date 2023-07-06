namespace DataFlow.Example.Services
{
    public interface IFakeTelemetry
    {
        Task PostTelemetryAsync(string itemIdToPost, CancellationToken cancellationToken);
    }
}
