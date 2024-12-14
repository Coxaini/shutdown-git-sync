namespace Shutdown.Monitor.Sync.Common.Configs;

public class ShutDownAddressConfig
{
    public const string ShutDownAddress = "ShutDownAddress";
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string House { get; init; } = string.Empty;
}