using System.Text.RegularExpressions;

namespace Shutdown.Monitor.Schedule.Models;

public class Street
{
    public int Id { get; init; }
    public string Name { get; init; }

    public Street(int id, string name)
    {
        Id = id;
        Name = name;
    }
}