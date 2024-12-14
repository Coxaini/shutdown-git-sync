using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Interfaces;

public interface IScheduleGroupService
{
    Task<int> GetAddressGroupAsync(Address address);
}