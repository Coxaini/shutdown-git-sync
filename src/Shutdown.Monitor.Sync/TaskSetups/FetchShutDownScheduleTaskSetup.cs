using Microsoft.Extensions.Options;
using Quartz;
using Shutdown.Monitor.Sync.Tasks;

namespace Shutdown.Monitor.Sync.TaskSetups;

public class FetchShutDownScheduleTaskSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(FetchShutDownScheduleTask));
        options.AddJob<FetchShutDownScheduleTask>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInHours(1)
                        .RepeatForever())
            );
    }
}