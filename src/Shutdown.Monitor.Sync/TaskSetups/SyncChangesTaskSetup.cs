using Microsoft.Extensions.Options;
using Quartz;
using Shutdown.Monitor.Sync.Tasks;

namespace Shutdown.Monitor.Sync.TaskSetups;

public class SyncChangesTaskSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(SyncChangesTask));
        options.AddJob<SyncChangesTask>(builder => builder.WithIdentity(jobKey)
            .StoreDurably());
    }
}