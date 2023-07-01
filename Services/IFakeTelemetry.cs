namespace DataFlow.Example.Services
{
    public interface IFakeTelemetry
    {
        Task PostTelemetry(string input, CancellationToken cancellationToken);
    }
}
