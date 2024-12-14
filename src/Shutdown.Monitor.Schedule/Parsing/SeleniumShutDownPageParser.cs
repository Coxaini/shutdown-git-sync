using Microsoft.Extensions.Options;
using OpenQA.Selenium.Chrome;
using Shutdown.Monitor.Schedule.Common.Configs;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;

namespace Shutdown.Monitor.Schedule.Parsing;

public class SeleniumShutDownPageParser : IShutDownSiteParser
{
    private readonly ISeleniumShutDownPageParsingStrategy _parsingStrategy;
    private readonly Uri _siteUri;


    public SeleniumShutDownPageParser(ISeleniumShutDownPageParsingStrategy parsingStrategy, IOptions<ShutDownServiceConfig> config)
    {
        _parsingStrategy = parsingStrategy;
        _siteUri = new Uri(config.Value.SiteUrl);
    }

    public async Task<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(DateOnly date)
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-position=-32000,-32000");
        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        
        var webDriver = new ChromeDriver(service, options);
        await webDriver.Navigate().GoToUrlAsync(_siteUri);

        var result = await _parsingStrategy.RetrieveGroupScheduleAsync(webDriver, date);

        webDriver.Quit();

        return result;
    }
}