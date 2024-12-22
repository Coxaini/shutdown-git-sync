namespace Shutdown.Monitor.Sync.Interfaces;

public interface ISyncChangesScheduler
{
    Task Schedule(IEnumerable<TimeOnly> times);
    Task ScheduleImmediate();
}