using Quartz;

namespace Shutdown.Monitor.Sync.Tasks;

public class SyncChangesTask : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.CompletedTask;
    }
}