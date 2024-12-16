using Shutdown.Monitor.Schedule.Clients.Interfaces;
using Shutdown.Monitor.Schedule.Interfaces;
using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Services;

public class ScheduleGroupService : IScheduleGroupService
{
    private readonly IShutDownApiClient _shutDownApiClient;

    public ScheduleGroupService(IShutDownApiClient shutDownApiClient)
    {
        _shutDownApiClient = shutDownApiClient;
    }

    public async Task<GroupId> GetAddressGroupAsync(Address address)
    {
        var streets = await _shutDownApiClient.GetStreetsAsync(address.City);
        var street = streets.FirstOrDefault(s => s.Name == address.Street);

        if (street is null)
        {
            throw new ArgumentException($"Street {address.Street} not found in city {address.City}");
        }

        var houses = await _shutDownApiClient.GetHousesAsync(address.City, street.Id);
        var house = houses.FirstOrDefault(h => h.Name == address.House);

        if (house is null)
        {
            throw new ArgumentException($"House {address.House} not found on street {address.Street}");
        }

        return house.GroupId;
    }
}