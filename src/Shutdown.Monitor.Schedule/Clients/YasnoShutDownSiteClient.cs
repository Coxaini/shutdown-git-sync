using Microsoft.Extensions.Options;
using Shutdown.Monitor.Schedule.Clients.Interfaces;
using Shutdown.Monitor.Schedule.Common.Configs;
using Shutdown.Monitor.Schedule.Interfaces;

namespace Shutdown.Monitor.Schedule.Clients;

public class YasnoShutDownSiteClient : IShutDownSiteClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _siteUri;

    public YasnoShutDownSiteClient(HttpClient httpClient, IOptions<ShutDownServiceConfig> config)
    {
        _httpClient = httpClient;
        _siteUri = new Uri(config.Value.SiteUrl);
    }

    public async Task<string> GetSiteContentAsync()
    {
        var response = await _httpClient.GetAsync(_siteUri);
        return await response.Content.ReadAsStringAsync();
    }
}