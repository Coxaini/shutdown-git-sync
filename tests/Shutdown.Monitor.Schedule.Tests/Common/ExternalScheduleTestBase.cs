using Microsoft.Extensions.Configuration;

namespace Shutdown.Monitor.Schedule.Tests.Common;

public abstract class ExternalScheduleTestBase
{
    protected readonly ExternalScheduleConfig Configuration;

    protected ExternalScheduleTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .AddUserSecrets<ExternalScheduleConfig>()
            .Build();

        var config = configuration.Get<ExternalScheduleConfig>();

        Configuration = config ?? throw new ArgumentNullException(nameof(ExternalScheduleConfig));
    }
}