namespace Shutdown.Monitor.Schedule.Common.Configs;

public class ShutDownServiceConfig
{
    public const string ShutDownService = "ShutDownService";
    public string SiteUrl { get; init; } = string.Empty;
    public string BaseApiUrl { get; init; } = string.Empty;
}