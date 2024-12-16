using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using Shutdown.Monitor.Schedule.Mappers;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;

namespace Shutdown.Monitor.Schedule.Parsing.Strategies;

public partial class YasnoHtmlShutDownPageParsingStrategy : IHtmlShutDownPageParsingStrategy
{
    private const int IntervalCount = 25;

    public IEnumerable<GroupSchedule> RetrieveGroupSchedule(IDocument page)
    {
        var scheduleBlock = page.QuerySelector("div.schedule-item");
        Guard.Against.Null(scheduleBlock, nameof(scheduleBlock));

        scheduleBlock = page.QuerySelector("div.schedule-item");
        Guard.Against.Null(scheduleBlock, nameof(scheduleBlock));

        var schedule = scheduleBlock.QuerySelector("div.schedule");
        Guard.Against.Null(schedule, nameof(schedule));

        var scheduleItems = schedule.QuerySelectorAll("div[class=\"col\"], div[class=\"col legend\"]").ToArray();

        var groups = ExtractGroupSchedules(scheduleItems);

        return groups;
    }

    private static List<GroupSchedule> ExtractGroupSchedules(IElement[] scheduleItems)
    {
        var groups = new List<GroupSchedule>();
        int groupCount = scheduleItems.Length / IntervalCount;
        for (var i = 0; i < groupCount; i++)
        {
            var groupItems = new Span<IElement>(scheduleItems, i * IntervalCount, IntervalCount);
            var lightOffIntervals = new List<TimeRange>();

            var groupId = groupItems[0].TextContent.ToGroupId();
            for (int j = 1; j < IntervalCount; j++)
            {
                var groupItem = groupItems[j];
                var isOff = groupItems[j].QuerySelector(".blackout") != null;

                if (!isOff) continue;

                var time = TimeSpan.FromHours(j);
                var timeRange = new TimeRange(
                    TimeOnly.FromTimeSpan(time),
                    TimeOnly.FromTimeSpan(time.Add(TimeSpan.FromHours(1)))
                );
                lightOffIntervals.Add(timeRange);
            }

            var groupSchedule = new GroupSchedule(groupId, lightOffIntervals);
            groups.Add(groupSchedule);
        }

        return groups;
    }

    private static readonly Regex DateRegex = GeneratedDateRegex();

    [GeneratedRegex(@"\d{2}\.\d{2}\.\d{4}")]
    private static partial Regex GeneratedDateRegex();
}