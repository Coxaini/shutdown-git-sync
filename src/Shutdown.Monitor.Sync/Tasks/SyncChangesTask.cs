using Quartz;
using Shutdown.Monitor.Git.Interfaces;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Models;

namespace Shutdown.Monitor.Sync.Tasks;

public class SyncChangesTask : IJob
{
    private readonly IGitChangesSender _gitChangesSender;
    private readonly ITriggerSyncHttpClient _triggerSyncHttpClient;
    private readonly ILogger<SyncChangesTask> _logger;

    public SyncChangesTask(IGitChangesSender gitChangesSender, ITriggerSyncHttpClient triggerSyncHttpClient,
        ILogger<SyncChangesTask> logger)
    {
        _gitChangesSender = gitChangesSender;
        _triggerSyncHttpClient = triggerSyncHttpClient;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var gitResult = _gitChangesSender.SaveChangesToTempBranch();
        if (gitResult.IsFailure)
        {
            _logger.LogError("Failed to save changes to temp branch. Error: {Error}", gitResult.Exception);
            return;
        }

        var value = gitResult.Value;
        var syncRequest = new SyncRequest(value.LastBranchName, value.TempBranchName,
            value.CommitedFiles.Select(f => new SyncFile(f.FilePath, f.WasUntracked)));

        await _triggerSyncHttpClient.TriggerSync(syncRequest);
    }
}