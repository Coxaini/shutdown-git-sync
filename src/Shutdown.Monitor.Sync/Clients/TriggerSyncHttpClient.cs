using Shutdown.Monitor.Sync.Common.Constants;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Models;

namespace Shutdown.Monitor.Sync.Clients;

public class TriggerSyncHttpClient : ITriggerSyncHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TriggerSyncHttpClient> _logger;

    public TriggerSyncHttpClient(HttpClient httpClient, ILogger<TriggerSyncHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task TriggerSync(SyncRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(ApiRoutes.SyncWithGit, request);

        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to trigger sync with git. Status code: {StatusCode}", result.StatusCode);
        }
    }
}