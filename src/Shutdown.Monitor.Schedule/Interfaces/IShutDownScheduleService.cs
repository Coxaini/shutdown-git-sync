using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Interfaces;

public interface IShutDownScheduleService
{
    Task<GroupSchedule> GetShutDownScheduleAsync(Address address);
}