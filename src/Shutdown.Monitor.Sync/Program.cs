using Shutdown.Monitor.Schedule;
using Shutdown.Monitor.Sync;
using Shutdown.Monitor.Sync.Tasks;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddApp(builder.Configuration)
    .AddShutDownScheduleService(builder.Configuration);

var host = builder.Build();

host.Run();