using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Shutdown.Monitor.Schedule.Clients.Interfaces;
using Shutdown.Monitor.Schedule.Mappers;
using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Clients;

public class YasnoShutDownApiClient : IShutDownApiClient
{
    private const string Route = "/api/v1/electricity-outages-schedule";
    private readonly HttpClient _httpClient;

    public YasnoShutDownApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Street>> GetStreetsAsync(string city)
    {
        var url = $"{Route}/streets?region={city}";
        var response = await _httpClient.GetAsync(url);
        var results = await response.Content.ReadFromJsonAsync<IEnumerable<RawStreet>>();
        if (results == null) throw new Exception("Failed to parse streets");

        return results.Select(x => x.MapToStreet());
    }

    public async Task<IEnumerable<House>> GetHousesAsync(string city, int streetId)
    {
        var url = $"{Route}/houses?region={city}&street_id={streetId}";
        var response = await _httpClient.GetAsync(url);
        var results = await response.Content.ReadFromJsonAsync<IEnumerable<RawHouse>>();
        if (results == null) throw new Exception("Failed to parse houses");

        return results.Select(x => x.MapToHouse());
    }

    public class RawStreet
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }

    public class RawHouse
    {
        [JsonPropertyName("group")] public string GroupId { get; init; } = null!;

        public string Name { get; init; } = null!;
    }
}