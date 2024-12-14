using Microsoft.Extensions.Options;
using Shutdown.Monitor.Schedule.Common.Configs;
using Shutdown.Monitor.Schedule.Parsing;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;
using Shutdown.Monitor.Schedule.Parsing.Strategies;
using Shutdown.Monitor.Schedule.Tests.Common;

namespace Shutdown.Monitor.Schedule.Tests;

public class YasnoShutDownSiteSeleniumParserTests : ExternalScheduleTestBase
{
    private readonly IShutDownSiteParser _parser;

    public YasnoShutDownSiteSeleniumParserTests()
    {
        var options = Options.Create(new ShutDownServiceConfig()
        {
            SiteUrl = Configuration.SiteUrl
        });
        _parser = new SeleniumShutDownPageParser(new YasnoSeleniumShutDownPageParsingStrategy(), options);
    }

    [Fact]
    public async Task RetrieveGroupScheduleAsync_ShouldReturnGroupSchedule()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);

        var result = await _parser.RetrieveGroupScheduleAsync(date);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}