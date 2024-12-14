using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shutdown.Monitor.Schedule.Clients;
using Shutdown.Monitor.Schedule.Clients.Interfaces;
using Shutdown.Monitor.Schedule.Common.Configs;
using Shutdown.Monitor.Schedule.Interfaces;
using Shutdown.Monitor.Schedule.Parsing;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;
using Shutdown.Monitor.Schedule.Parsing.Strategies;
using Shutdown.Monitor.Schedule.Services;

namespace Shutdown.Monitor.Schedule;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddShutDownScheduleService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IShutDownScheduleService, ShutDownScheduleService>();

        var shutDownServiceConfig = new ShutDownServiceConfig();
        configuration.Bind(ShutDownServiceConfig.ShutDownService, shutDownServiceConfig);
        services.AddSingleton(Options.Create(shutDownServiceConfig));

        services.AddHttpClient<IShutDownApiClient, YasnoShutDownApiClient>(
            u => u.BaseAddress = new Uri(shutDownServiceConfig.BaseApiUrl));
        services.AddHttpClient<IShutDownSiteClient, YasnoShutDownSiteClient>();

        services.AddScoped<IShutDownSiteParser, HtmlShutDownPageParser>();
        services.AddScoped<IScheduleGroupService, ScheduleGroupService>();
        services.AddSingleton<IHtmlShutDownPageParsingStrategy, YasnoHtmlShutDownPageParsingStrategy>();
        return services;
    }
}