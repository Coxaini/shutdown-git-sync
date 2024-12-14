namespace Shutdown.Monitor.Schedule.Parsing.Exceptions;

public class ScheduleForDateNotFound : Exception
{
    public ScheduleForDateNotFound(DateOnly date) : base($"No schedule found for date {date}")
    {
    }
}