using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shutdown.Monitor.Cli.Clients;
using Shutdown.Monitor.Cli.Interfaces;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddHttpClient<ITriggerSyncHttpClient, TriggerSyncHttpClient>((provider, client) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var address = configuration["TriggerAddress"] ?? throw new ArgumentNullException($"TriggerAddress");

    client.BaseAddress = new Uri(address);
});
var app = builder.Build();

app.AddCommand(async (ITriggerSyncHttpClient client) => { await client.TriggerSync(); });

app.Run();