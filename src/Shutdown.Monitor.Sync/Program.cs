using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Shutdown.Monitor.Schedule;
using Shutdown.Monitor.Sync;
using Shutdown.Monitor.Sync.Common.Constants;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddCommandLine(source =>
    {
        for (var i = 0; i < args.Length; i++)
        {
            const string repositoryRepoPath = "--Repository:RepoPath=";
            if (!args[i].StartsWith(repositoryRepoPath, StringComparison.OrdinalIgnoreCase)) continue;

            var value = args[i].AsSpan().TrimStart(repositoryRepoPath.AsSpan());
            if (Path.IsPathRooted(value)) continue;
            var currentDirectory = AppContext.BaseDirectory;
            args[i] = $"--Repository:RepoPath={Path.Combine(currentDirectory, value.ToString())}";
        }

        source.Args = args;
    });

builder.Services
    .AddApp(builder.Configuration)
    .AddShutDownScheduleService(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPost(ApiRoutes.SyncWithGit,
    (SyncRequest request, [FromServices] ISyncChangesTriggerService triggerService) =>
    {
        triggerService.SyncChanges(request);
    });

app.MapPost(ApiRoutes.TriggerSync,
    async ([FromServices] ISyncChangesScheduler scheduler) => { await scheduler.ScheduleImmediate(); });


app.Run();