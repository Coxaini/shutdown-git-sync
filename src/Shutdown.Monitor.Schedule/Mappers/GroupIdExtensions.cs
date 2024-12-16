using Shutdown.Monitor.Schedule.Models;

namespace Shutdown.Monitor.Schedule.Mappers;

public static class GroupIdExtensions
{
    public static GroupId ToGroupId(this string groupId)
    {
        var groupSections = groupId.Split('.');
        return groupSections.Length switch
        {
            1 => new GroupId(int.Parse(groupSections[0])),
            2 => new GroupId(int.Parse(groupSections[0]), int.Parse(groupSections[1])),
            _ => throw new ArgumentException("Invalid group id format")
        };
    }
}