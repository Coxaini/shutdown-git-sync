namespace Shutdown.Monitor.Schedule.Models;

public class GroupSchedule
{
    public GroupSchedule(GroupId groupId, IReadOnlyList<TimeRange> timeRanges)
    {
        GroupId = groupId;
        TimeRanges = MergeTimeRanges(timeRanges);
    }

    private static List<TimeRange> MergeTimeRanges(IReadOnlyList<TimeRange> timeRanges)
    {
        if (timeRanges.Count == 0)
        {
            return [];
        }

        var unionTimeRanges = new List<TimeRange>();

        var start = timeRanges[0].Start;
        var end = timeRanges[0].End;
        for (var i = 1; i < timeRanges.Count; i++)
        {
            if (i == timeRanges.Count - 1)
            {
                unionTimeRanges.Add(new TimeRange(start, timeRanges[i].End));
                break;
            }

            if (end != timeRanges[i].Start)
            {
                unionTimeRanges.Add(new TimeRange(start, end));
                start = timeRanges[i].Start;
            }

            end = timeRanges[i].End;
        }

        return unionTimeRanges;
    }

    public GroupId GroupId { get; init; }
    public IReadOnlyList<TimeRange> TimeRanges { get; init; }
}