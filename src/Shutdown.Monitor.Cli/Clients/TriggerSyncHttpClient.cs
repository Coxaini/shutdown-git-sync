using Shutdown.Monitor.Cli.Interfaces;

namespace Shutdown.Monitor.Cli.Clients;

public class TriggerSyncHttpClient : ITriggerSyncHttpClient
{
    private readonly HttpClient _httpClient;

    public TriggerSyncHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task TriggerSync()
    {
        var result = await _httpClient.PostAsync("trigger-sync", null);

        result.EnsureSuccessStatusCode();
    }
}