using Microsoft.Extensions.Options;
using Quartz;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Interfaces;
using Shutdown.Monitor.Git.Services;
using Shutdown.Monitor.Sync.Clients;
using Shutdown.Monitor.Sync.Common;
using Shutdown.Monitor.Sync.Common.Configs;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Schedulers;
using Shutdown.Monitor.Sync.Services;
using Shutdown.Monitor.Sync.Tasks;
using Shutdown.Monitor.Sync.TaskSetups;

namespace Shutdown.Monitor.Sync;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ShutDownAddressConfig>(configuration.GetSection(ShutDownAddressConfig.ShutDownAddress));
        services.Configure<ScheduleConfig>(configuration.GetSection(ScheduleConfig.Schedule));
        services.Configure<GitConfig>(configuration.GetSection("Repository"));
        services.Configure<NetworkConfig>(configuration.GetSection(NetworkConfig.Network));

        services.AddSingleton<IGitChangesSender>(provider =>
        {
            var gitConfig = provider.GetRequiredService<IOptions<GitConfig>>().Value;
            return new GitChangesSender(gitConfig);
        });

        services.AddSingleton<IGitChangesReceiver>(provider =>
        {
            var gitConfig = provider.GetRequiredService<IOptions<GitConfig>>().Value;
            return new GitChangesReceiver(gitConfig);
        });

        services.AddScoped<ISyncChangesTriggerService, SyncChangesTriggerService>();
        services.AddScoped<ISyncChangesScheduler, SyncChangesScheduler>();

        services.AddQuartz();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.ConfigureOptions<FetchShutDownScheduleTaskSetup>();
        services.ConfigureOptions<SyncChangesTaskSetup>();

        services.AddHttpClient<ITriggerSyncHttpClient, TriggerSyncHttpClient>((provider, client) =>
        {
            var networkConfig = provider.GetRequiredService<IOptions<NetworkConfig>>().Value;
            client.BaseAddress = new Uri(networkConfig.SyncAddress);
        });

        return services;
    }
}