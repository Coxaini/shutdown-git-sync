using LibGit2Sharp;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Models;
using Shutdown.Monitor.Git.Services;
using Shutdown.Monitor.Git.Tests.Common;

namespace Shutdown.Monitor.Git.Tests;

public class GitChangesReceiverTests : GitTestBase, IDisposable
{
    private readonly GitChangesReceiver _gitChangesReceiver;
    private readonly Repository _receiverRepository;
    private readonly Repository _senderRepository;

    public GitChangesReceiverTests()
    {
        var receiverRepoPath = Path.Combine(AppContext.BaseDirectory, GitTestConstants.ReceiverRepoDir);

        if (!Directory.Exists(receiverRepoPath))
            CloneRepository(CredentialsHandler, RemoteGitConfig.Origin, receiverRepoPath);

        _receiverRepository = new Repository(receiverRepoPath);

        var senderRepoPath = Path.Combine(AppContext.BaseDirectory, GitTestConstants.SenderRepoDir);

        if (!Directory.Exists(senderRepoPath))
            CloneRepository(CredentialsHandler, RemoteGitConfig.Origin, senderRepoPath);

        _senderRepository = new Repository(senderRepoPath);

        _gitChangesReceiver = new GitChangesReceiver(new GitConfig
        {
            RepoPath = receiverRepoPath,
            Origin = RemoteGitConfig.Origin,
            TempBranchPrefix = RemoteGitConfig.TempBranchPrefix,
            UserName = RemoteGitConfig.UserName,
            Password = RemoteGitConfig.Password,
            Name = RemoteGitConfig.Name,
            Email = RemoteGitConfig.Email
        });
    }

    [Fact]
    public void ApplyChanges_ShouldUnpackTempBranch_WithTrackedFile()
    {
        var file = Path.Combine(_senderRepository.Info.WorkingDirectory, "test-file.txt");
        CreateBranchWithCommit(_senderRepository, "test-branch", file, "test content", "test commit message");
        CreateBranchWithCommit(_senderRepository, "temp-test-branch", file, "temp test content",
            "temp test commit message");
        Commands.Pull(_senderRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);
        Commands.Pull(_receiverRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        CommitedFile[] commitedFiles = [new("test-file.txt", false)];
        _gitChangesReceiver.ApplyChangesFromTempBranch(
            commitedFiles,
            "refs/remotes/origin/test-branch",
            "refs/remotes/origin/temp-test-branch");

        var branch = _receiverRepository.Branches["test-branch"];
        Assert.NotNull(branch);
        Assert.Equal("test commit message\n", branch.Tip.Message);
        Assert.Equal("test content" + "temp test content", File.ReadAllText(file));
    }

    public override void Dispose()
    {
        ClearRepository(_senderRepository, true);
        ClearRepository(_receiverRepository, false);
    }
}