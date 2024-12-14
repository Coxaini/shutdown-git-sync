using Quartz;
using Shutdown.Monitor.Sync.Common;
using Shutdown.Monitor.Sync.Common.Configs;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Schedulers;
using Shutdown.Monitor.Sync.Tasks;
using Shutdown.Monitor.Sync.TaskSetups;

namespace Shutdown.Monitor.Sync;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ShutDownAddressConfig>(configuration.GetSection(ShutDownAddressConfig.ShutDownAddress));
        services.Configure<ScheduleConfig>(configuration.GetSection(ScheduleConfig.Schedule));
        services.AddScoped<ISyncChangesScheduler, SyncChangesScheduler>();
        services.AddQuartz();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.ConfigureOptions<FetchShutDownScheduleTaskSetup>();
        services.ConfigureOptions<SyncChangesTaskSetup>();

        return services;
    }
}