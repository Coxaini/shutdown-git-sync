using Quartz;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Tasks;

namespace Shutdown.Monitor.Sync.Schedulers;

[DisallowConcurrentExecution]
public class SyncChangesScheduler : ISyncChangesScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<SyncChangesScheduler> _logger;

    public SyncChangesScheduler(ISchedulerFactory schedulerFactory, ILogger<SyncChangesScheduler> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task Schedule(IEnumerable<TimeOnly> times)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var existingTriggers =
            await scheduler.GetTriggersOfJob(JobKey.Create(nameof(SyncChangesTask)));

        foreach (var trigger in existingTriggers)
        {
            await scheduler.UnscheduleJob(trigger.Key);
        }

        foreach (var time in times)
        {
            var timeTrigger = TriggerBuilder.Create()
                .WithIdentity(time.ToString())
                .ForJob(JobKey.Create(nameof(SyncChangesTask)))
                .StartAt(DateTime.Today.Add(time.ToTimeSpan()))
                .Build();

            await scheduler.ScheduleJob(timeTrigger);

            _logger.LogInformation("Scheduled SyncChangesTask at {Time}", time);
        }
    }

    public async Task ScheduleImmediate()
    {
        var immediateTrigger = TriggerBuilder.Create()
            .WithIdentity("Immediate")
            .ForJob(JobKey.Create(nameof(SyncChangesTask)))
            .StartNow()
            .Build();

        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(immediateTrigger);

        _logger.LogInformation("Scheduled SyncChangesTask immediately");
    }
}