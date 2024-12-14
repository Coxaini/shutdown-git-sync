using Microsoft.Extensions.Options;
using Quartz;
using Shutdown.Monitor.Schedule.Interfaces;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Sync.Common;
using Shutdown.Monitor.Sync.Common.Configs;
using Shutdown.Monitor.Sync.Interfaces;

namespace Shutdown.Monitor.Sync.Tasks;

public class FetchShutDownScheduleTask : IJob
{
    private readonly Address _address;
    private readonly ILogger<FetchShutDownScheduleTask> _logger;
    private readonly IShutDownScheduleService _shutDownScheduleService;
    private readonly ScheduleConfig _scheduleConfig;
    private readonly ISyncChangesScheduler _syncChangesScheduler;

    public FetchShutDownScheduleTask(ILogger<FetchShutDownScheduleTask> logger,
        IShutDownScheduleService shutDownScheduleService,
        IOptions<ShutDownAddressConfig> addressConfig, IOptions<ScheduleConfig> scheduleConfig,
        ISyncChangesScheduler syncChangesScheduler)

    {
        _logger = logger;
        _shutDownScheduleService = shutDownScheduleService;
        _syncChangesScheduler = syncChangesScheduler;
        var config = addressConfig.Value;
        _address = new Address(config.Street, config.City, config.House);
        _scheduleConfig = scheduleConfig.Value;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Fetching shut down schedule");

        var schedule = await _shutDownScheduleService.GetShutDownScheduleAsync(_address);

        var futureTimeRanges = GetFutureTimeRangesSyncTimes(schedule,
            _scheduleConfig.DeviceType is ElectricityDeviceType.Battery);

        await _syncChangesScheduler.Schedule(futureTimeRanges);

        _logger.LogInformation("Shut down schedule fetched");
    }

    private static IEnumerable<TimeOnly> GetFutureTimeRangesSyncTimes(GroupSchedule schedule, bool byEnd = false)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        return schedule.TimeRanges
            .Where(tr => tr.Start > now || (byEnd && tr.End > now))
            .Select(tr => byEnd ? tr.End : tr.Start);
    }
}