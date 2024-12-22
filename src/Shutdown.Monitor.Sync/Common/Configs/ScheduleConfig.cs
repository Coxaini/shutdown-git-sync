namespace Shutdown.Monitor.Sync.Common.Configs;

public class ScheduleConfig
{
    public const string Schedule = "Schedule";
    public ElectricityDeviceType DeviceType { get; init; }
    public int TimeOffSet { get; init; }
    public TimeSpan TimeOffSetSpan => TimeSpan.FromMinutes(TimeOffSet);
}