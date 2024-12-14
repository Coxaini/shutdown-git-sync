using AngleSharp.Dom;
using OpenQA.Selenium;
using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Parsing.Interfaces;

public interface ISeleniumShutDownPageParsingStrategy
{
    /// <summary>
    /// Retrieve group schedule from page
    /// </summary>
    /// <param name="driver">web driver</param>
    /// <param name="date">date to retrieve schedule for</param>
    /// <returns>group schedule for the date</returns>
    /// <exception cref="Shutdown.Monitor.Schedule.Parsing.Exceptions.ScheduleForDateNotFound">thrown when no schedule found for the date</exception>
    //ValueTask<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(IDocument page,DateOnly date);
     Task<IEnumerable<GroupSchedule>> RetrieveGroupScheduleAsync(IWebDriver driver, DateOnly date);
}