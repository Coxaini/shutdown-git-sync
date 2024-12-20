using LibGit2Sharp;
using Shutdown.Monitor.Git.Common.Configs;
using Shutdown.Monitor.Git.Exceptions;
using Shutdown.Monitor.Git.Services;
using Shutdown.Monitor.Git.Tests.Common;

namespace Shutdown.Monitor.Git.Tests;

public class GitChangesSenderTests : GitTestBase
{
    private readonly GitChangesSender _gitChangesSender;
    private readonly Repository _senderRepository;
    private readonly string _testRepoPath;

    public GitChangesSenderTests()
    {
        _testRepoPath = Path.Combine(AppContext.BaseDirectory, GitTestConstants.SenderRepoDir);

        if (!Directory.Exists(_testRepoPath))
            CloneRepository(CredentialsHandler, RemoteGitConfig.Origin, _testRepoPath);

        _senderRepository = new Repository(_testRepoPath);

        _gitChangesSender = new GitChangesSender(new GitConfig
        {
            RepoPath = _testRepoPath,
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
        var file = Path.Combine(_testRepoPath, "test-file.txt");
        File.AppendAllText(file, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(_senderRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.NotNull(_senderRepository.Branches[result.Value.TempBranchName]);
    }

    [Fact]
    public void SaveChanges_ShouldContainUntrackedFile()
    {
        var file = Path.Combine(_testRepoPath, "test-file.txt");
        File.WriteAllText(file, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(_senderRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value.CommitedFiles, f => f.WasUntracked);
    }

    [Fact]
    public void SaveChanges_ShouldContainAddedFile()
    {
        var filePath = Path.Combine(_testRepoPath, "test-file.txt");
        CreateBranchWithCommit(_senderRepository, "test-branch", filePath, "test content");
        File.AppendAllText(filePath, "test");

        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(_senderRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value.CommitedFiles, f => !f.WasUntracked);
    }

    [Fact]
    public void SaveChanges_ShouldReturnAnError_WhenNoChangesInRepository()
    {
        var result = _gitChangesSender.SaveChangesToTempBranch();
        Commands.Pull(_senderRepository, new Signature(Identity, DateTimeOffset.Now), PullOptions);

        Assert.True(result.IsFailure);
        Assert.IsType<NoChangesInRepository>(result.Exception);
    }

    public override void Dispose()
    {
        ClearRepository(_senderRepository);
    }
}