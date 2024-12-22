using Microsoft.Extensions.Options;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Interfaces;
using Shutdown.Monitor.Git.Models;
using Shutdown.Monitor.Sync.Interfaces;
using Shutdown.Monitor.Sync.Models;

namespace Shutdown.Monitor.Sync.Services;

public class SyncChangesTriggerService : ISyncChangesTriggerService
{
    private readonly IGitChangesReceiver _gitChangesReceiver;

    public SyncChangesTriggerService(IGitChangesReceiver gitChangesReceiver)
    {
        _gitChangesReceiver = gitChangesReceiver;
    }

    public void SyncChanges(SyncRequest syncRequest)
    {
        var commitedFiles = syncRequest.Files
            .Select(f => new CommitedFile(f.FilePath, f.WasUntracked));
        _gitChangesReceiver.ApplyChangesFromTempBranch(commitedFiles, syncRequest.BranchName,
            syncRequest.TempBranchName);
    }
}