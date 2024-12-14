namespace Shutdown.Monitor.Schedule.Models;

public class House
{
    public int GroupId { get; init; }
    public string Name { get; init; }
    public House(string name, int groupId)
    {
        Name = name;
        GroupId = groupId;
    }
}