using Microsoft.Extensions.Options;
using Shutdown.Monitor.Schedule.Clients;
using Shutdown.Monitor.Schedule.Common.Configs;
using Shutdown.Monitor.Schedule.Parsing;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;
using Shutdown.Monitor.Schedule.Parsing.Strategies;
using Shutdown.Monitor.Schedule.Tests.Common;

namespace Shutdown.Monitor.Schedule.Tests;

public class YasnoShutDownSiteHtmlParserTests : ExternalScheduleTestBase
{
    private readonly IShutDownSiteParser _parser;

    public YasnoShutDownSiteHtmlParserTests()
    {
        var client = new YasnoShutDownSiteClient(new HttpClient(),
            Options.Create(new ShutDownServiceConfig
            {
                SiteUrl = Configuration.SiteUrl
            }));
        _parser = new HtmlShutDownPageParser(client, new YasnoHtmlShutDownPageParsingStrategy());
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