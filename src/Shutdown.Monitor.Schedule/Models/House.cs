namespace Shutdown.Monitor.Schedule.Models;

public class House
{
    public GroupId GroupId { get; init; }
    public string Name { get; init; }

    public House(string name, GroupId groupId)
    {
        Name = name;
        GroupId = groupId;
    }
}