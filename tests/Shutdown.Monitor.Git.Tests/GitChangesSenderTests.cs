using LibGit2Sharp;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Exceptions;
using Shutdown.Monitor.Git.Services;
using Shutdown.Monitor.Git.Tests.Common;

namespace Shutdown.Monitor.Git.Tests;

public class GitChangesSenderTests : GitTestBase
{
    private readonly GitChangesSender _gitChangesSender;

    public GitChangesSenderTests()
    {
        _gitChangesSender = new GitChangesSender(new GitConfig
        {
            RepoPath = RemoteGitConfig.RepoPath,
            Origin = RemoteGitConfig.Origin,
            TempBranchPrefix = RemoteGitConfig.TempBranchPrefix,
            UserName = RemoteGitConfig.UserName,
            Password = RemoteGitConfig.Password,
            Name = RemoteGitConfig.Name,
            Email = RemoteGitConfig.Email
        });
    }

    [Fact]
    public void SaveChanges_ShouldCreateTempBranchOnRemote()
    {
        var file = Path.Combine(RemoteGitConfig.RepoPath, "test-file.txt");
        File.AppendAllText(file, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(Repository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.NotNull(Repository.Branches[result.Value.BranchName]);
    }

    [Fact]
    public void SaveChanges_ShouldContainUntrackedFile()
    {
        var file = Path.Combine(RemoteGitConfig.RepoPath, "test-file.txt");
        File.WriteAllText(file, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(Repository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value.CommitedFiles, f => f.WasUntracked);
    }

    [Fact]
    public void SaveChanges_ShouldContainAddedFile()
    {
        CreateTestBranchWithCommit("test-branch", "test-file.txt", "test content");
        var file = Path.Combine(RemoteGitConfig.RepoPath, "test-file.txt");
        File.AppendAllText(file, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(Repository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value.CommitedFiles, f => !f.WasUntracked);
    }

    [Fact]
    public void SaveChanges_ShouldReturnAnError_WhenNoChangesInRepository()
    {
        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(Repository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsFailure);
        Assert.IsType<NoChangesInRepository>(result.Exception);
    }
}