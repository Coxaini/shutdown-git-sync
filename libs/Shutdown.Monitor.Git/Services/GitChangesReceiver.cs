using LibGit2Sharp;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Interfaces;
using Shutdown.Monitor.Git.Models;

namespace Shutdown.Monitor.Git.Services;

public class GitChangesReceiver : GitChangesClient, IGitChangesReceiver
{
    public GitChangesReceiver(GitConfig configuration) : base(configuration)
    {
    }

    public void ApplyChangesFromTempBranch(IEnumerable<CommitedFile> commitedFiles, string branchName,
        string tempBranchName)
    {
        var remote = Repository.Network.Remotes["origin"];
        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(Repository, remote.Name, refSpecs, FetchOptions, string.Empty);

        var trackedtempBranch = Repository.Branches[tempBranchName];
        var localTempBranch = Repository.CreateBranch(GetLocalFriendlyNameFromRemoteBranch(trackedtempBranch),
            trackedtempBranch.Tip);

        Repository.Branches.Update(localTempBranch, b => b.Remote = remote.Name,
            b => b.UpstreamBranch = localTempBranch.CanonicalName);
        Commands.Checkout(Repository, localTempBranch);

        Repository.Reset(ResetMode.Hard, "HEAD~1");

        var trackedBranch = Repository.Branches[branchName];
        var localBranch =
            Repository.CreateBranch(GetLocalFriendlyNameFromRemoteBranch(trackedBranch), trackedBranch.Tip);

        Repository.Branches.Update(localBranch, b => b.Remote = remote.Name,
            b => b.UpstreamBranch = localBranch.CanonicalName);
        Commands.Checkout(Repository, localBranch);

        foreach (var file in commitedFiles)
        {
            if (file.WasUntracked)
            {
                Commands.Unstage(Repository, file.FilePath);
            }
        }

        Repository.Branches.Remove(localTempBranch);
        Repository.Network.Push(remote,
            $":{localTempBranch.CanonicalName}", PushOptions);
    }

    private string GetLocalFriendlyNameFromRemoteBranch(Branch branch)
    {
        return branch.FriendlyName.Replace($"{branch.RemoteName}/", "");
    }
}