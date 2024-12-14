using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Parsing.Interfaces;

public interface IShutDownSiteParser
{
    Task<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(DateOnly date);
}