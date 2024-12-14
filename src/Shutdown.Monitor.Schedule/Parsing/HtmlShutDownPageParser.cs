using AngleSharp;
using Shutdown.Monitor.Schedule.Clients.Interfaces;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;

namespace Shutdown.Monitor.Schedule.Parsing;

public class HtmlShutDownPageParser : IShutDownSiteParser
{
    private readonly IShutDownSiteClient _siteClient;
    private readonly IConfiguration _parserConfig;
    private readonly IHtmlShutDownPageParsingStrategy _parsingStrategy;

    public HtmlShutDownPageParser(IShutDownSiteClient siteClient, IHtmlShutDownPageParsingStrategy parsingStrategy)
    {
        _siteClient = siteClient;
        _parsingStrategy = parsingStrategy;
        _parserConfig = Configuration.Default.WithDefaultLoader();
    }

    public async Task<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(DateOnly date)
    {
        var content = await _siteClient.GetSiteContentAsync();

        var context = BrowsingContext.New(_parserConfig);
        
        var document = await context.OpenAsync(req => req.Content(content));
        
        return _parsingStrategy.RetrieveGroupSchedule(document);
    }
}
