using Shutdown.Monitor.Git.Models;

namespace Shutdown.Monitor.Git.Interfaces;

public interface IGitChangesReceiver
{
    void ApplyChangesFromTempBranch(IEnumerable<CommitedFile> commitedFiles, string branchName,
        string tempBranchName);
}