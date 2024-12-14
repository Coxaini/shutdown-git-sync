using System.Text.RegularExpressions;
using Shutdown.Monitor.Schedule.Clients;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Services;

namespace Shutdown.Monitor.Schedule.Mappers;

public static class HouseMapper
{
    public static House MapToHouse(this YasnoShutDownApiClient.RawHouse rawHouse)
    {
        return new House(Regex.Unescape(rawHouse.Name), rawHouse.GroupId);
    }
}