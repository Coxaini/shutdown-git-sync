﻿namespace Shutdown.Monitor.Schedule.Models;

public readonly struct TimeRange
{
    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    public TimeOnly Start { get; }
    public TimeOnly End { get; }
}