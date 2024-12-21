using LibGit2Sharp;
using Shutdown.Monitor.Git.Common;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Common.Constants;
using Shutdown.Monitor.Git.Common.Results;
using Shutdown.Monitor.Git.Exceptions;
using Shutdown.Monitor.Git.Interfaces;
using Shutdown.Monitor.Git.Models;

namespace Shutdown.Monitor.Git.Services;

public class GitChangesSender : GitChangesClient, IGitChangesSender
{
    public GitChangesSender(GitConfig configuration) : base(configuration)
    {
    }

    public GitResult<SaveChangesResult> SaveChangesToTempBranch()
    {
        var status = Repository.RetrieveStatus();

        var files = GetFileChanges(status);

        if (files.Length == 0)
            return new NoChangesInRepository();

        var currentBranch = Repository.Head;
        var tempBranchName =
            $"{Configuration.TempBranchPrefix}-temp-{currentBranch.FriendlyName}";

        var tempBranch = Repository.CreateBranch(tempBranchName, currentBranch.Tip);

        AddFiles(status);
        var remote = Repository.Network.Remotes["origin"];

        Commands.Checkout(Repository, tempBranch);
        Repository.Commit(GitConstants.TempCommitMessage, new Signature(Identity, DateTimeOffset.Now),
            new Signature(Identity, DateTimeOffset.Now));

        Repository.Branches.Update(tempBranch, b => b.Remote = remote.Name,
            b => b.UpstreamBranch = tempBranch.CanonicalName);

        Repository.Network.Push(tempBranch, PushOptions);

        return new SaveChangesResult(tempBranch.TrackedBranch.CanonicalName, currentBranch.TrackedBranch.CanonicalName,
            files);
    }

    private static CommitedFile[] GetFileChanges(RepositoryStatus status)
    {
        var files = status
            .Select(f => new CommitedFile(f.FilePath, f.State is FileStatus.NewInWorkdir))
            .ToArray();
        return files;
    }

    private void AddFiles(IEnumerable<StatusEntry> fileEntries)
    {
        foreach (var file in fileEntries)
        {
            Commands.Stage(Repository, file.FilePath);
        }
    }
}