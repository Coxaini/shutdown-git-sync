using System.Text.RegularExpressions;
using Shutdown.Monitor.Schedule.Clients;
using Shutdown.Monitor.Schedule.Models;
using Shutdown.Monitor.Schedule.Services;

namespace Shutdown.Monitor.Schedule.Mappers;

public static class StreetMapper
{
    public static Street MapToStreet(this YasnoShutDownApiClient.RawStreet rawStreet)
    {
        return new Street(rawStreet.Id, Regex.Unescape(rawStreet.Name));
    }
}