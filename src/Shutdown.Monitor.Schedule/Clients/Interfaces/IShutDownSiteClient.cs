namespace Shutdown.Monitor.Schedule.Clients.Interfaces;

public interface IShutDownSiteClient
{
    Task<string> GetSiteContentAsync();
}