using Shutdown.Monitor.Schedule.Interfaces;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;

namespace Shutdown.Monitor.Schedule.Services;

public class ShutDownScheduleService : IShutDownScheduleService
{
    private readonly IScheduleGroupService _scheduleGroupService;
    private readonly IShutDownSiteParser _shutDownSiteParser;

    public ShutDownScheduleService(IShutDownSiteParser shutDownSiteParser, IScheduleGroupService scheduleGroupService)
    {
        _shutDownSiteParser = shutDownSiteParser;
        _scheduleGroupService = scheduleGroupService;
    }

    public async Task<GroupSchedule> GetShutDownScheduleAsync(Address address)
    {
        var groupId = await _scheduleGroupService.GetAddressGroupAsync(address);

        var schedule = await _shutDownSiteParser.RetrieveGroupScheduleAsync(DateOnly.FromDateTime(DateTime.Now));

        return schedule.First(s => s.GroupId == groupId);
    }
}