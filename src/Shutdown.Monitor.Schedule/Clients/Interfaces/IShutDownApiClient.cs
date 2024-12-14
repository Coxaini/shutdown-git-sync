using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Clients.Interfaces;

public interface IShutDownApiClient
{
    Task<IEnumerable<Street>> GetStreetsAsync(string city);
    Task<IEnumerable<House>> GetHousesAsync(string city, int streetId);
}