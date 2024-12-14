﻿using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using OpenQA.Selenium;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Parsing.Exceptions;
using Shutdown.Monitor.Schedule.Parsing.Interfaces;

namespace Shutdown.Monitor.Schedule.Parsing.Strategies;

public partial class YasnoSeleniumShutDownPageParsingStrategy : ISeleniumShutDownPageParsingStrategy
{
    private const int GroupCount = 5;
    private const int IntervalCount = 24;

    private static readonly Regex DateRegex = GeneratedDateRegex();

    [GeneratedRegex(@"\d{2}\.\d{2}\.\d{4}")]
    private static partial Regex GeneratedDateRegex();

    public async Task<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(IWebDriver driver, DateOnly date)
    {
        var scheduleBlock = driver.FindElement(By.CssSelector("div.schedule-item"));
        Guard.Against.Null(scheduleBlock, nameof(scheduleBlock));

        var wrap = scheduleBlock.FindElement(By.CssSelector(".schedule-tabs-wrap"));
        Guard.Against.Null(wrap, nameof(wrap));

        var dateButtons = wrap.FindElements(By.TagName("button"));

        var buttonsWithDate = dateButtons.Select(button => new
        {
            Date = DateRegex.Match(button.Text).Value,
            Button = button
        });

        var button = buttonsWithDate.FirstOrDefault(x => x.Date == date.ToString("dd.MM.yyyy"))?.Button;

        if (button == null)
        {
            throw new ScheduleForDateNotFound(date);
        }

        var actions = new OpenQA.Selenium.Interactions.Actions(driver);
        actions.MoveToElement(button).Perform();

        button.Click();

        await Task.Delay(100);

        var schedule = scheduleBlock.FindElement(By.CssSelector("div.schedule"));
        Guard.Against.Null(schedule, nameof(schedule));

        var scheduleItems = schedule.FindElements(By.CssSelector("div[class=\"col\"]")).ToArray();

        var groups = ExtractGroupSchedules(scheduleItems);

        return groups;
    }

    private static List<GroupSchedule> ExtractGroupSchedules(IWebElement[] scheduleItems)
    {
        var groups = new List<GroupSchedule>();
        int groupCount = scheduleItems.Length / IntervalCount;
        for (var i = 0; i < groupCount; i++)
        {
            var groupItems = new Span<IWebElement>(scheduleItems, i * IntervalCount, IntervalCount);
            var lightOffIntervals = new List<TimeRange>();

            for (var j = 0; j < IntervalCount; j++)
            {
                var groupItem = groupItems[j];
                var isOff = groupItem.FindElements(By.CssSelector(".blackout")).Count != 0;

                if (!isOff) continue;

                var time = TimeSpan.FromHours(j);
                var timeRange = new TimeRange(
                    TimeOnly.FromTimeSpan(time),
                    TimeOnly.FromTimeSpan(time.Add(TimeSpan.FromHours(1)))
                );
                lightOffIntervals.Add(timeRange);
            }

            var groupSchedule = new GroupSchedule(i + 1, lightOffIntervals);
            groups.Add(groupSchedule);
        }

        return groups;
    }
}