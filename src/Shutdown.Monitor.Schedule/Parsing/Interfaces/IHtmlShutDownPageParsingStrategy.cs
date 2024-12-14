using AngleSharp.Dom;
using OpenQA.Selenium;
using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Parsing.Interfaces;

public interface IHtmlShutDownPageParsingStrategy
{
    /// <summary>
    /// Retrieve group schedule from page
    /// </summary>
    /// <param name="document">web document</param>
    /// <returns>group schedule for the date</returns>
    
     IEnumerable<GroupSchedule> RetrieveGroupSchedule(IDocument document);
}