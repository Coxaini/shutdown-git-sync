using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Configuration;

namespace Shutdown.Monitor.Git.Tests.Common;

public abstract class GitTestBase : IDisposable
{
    protected readonly RemoteGitConfig RemoteGitConfig;
    protected readonly CredentialsHandler CredentialsHandler;
    protected readonly PushOptions PushOptions;
    protected readonly PullOptions PullOptions;
    protected readonly FetchOptions FetchOptions;
    protected readonly Identity Identity;

    protected GitTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .AddUserSecrets<GitTestBase>()
            .Build();

        var config = configuration.Get<RemoteGitConfig>();

        RemoteGitConfig = config ?? throw new ArgumentNullException(nameof(RemoteGitConfig));

        CredentialsHandler = (url, usernameFromUrl, types) =>
            new UsernamePasswordCredentials
            {
                Username = RemoteGitConfig.UserName,
                Password = RemoteGitConfig.Password
            };

        PushOptions = new PushOptions
        {
            CredentialsProvider = CredentialsHandler
        };
        FetchOptions = new FetchOptions
        {
            CredentialsProvider = CredentialsHandler
        };
        PullOptions = new PullOptions()
        {
            FetchOptions = FetchOptions
        };

        Identity = new Identity(RemoteGitConfig.Name, RemoteGitConfig.Email);
    }

    protected void CloneRepository(CredentialsHandler credentialsHandler, string origin, string repoPath)
    {
        var cloneOptions = new CloneOptions
        {
            FetchOptions =
            {
                CredentialsProvider = credentialsHandler
            }
        };
        var directory = Directory.CreateDirectory(repoPath);
        Repository.Clone(origin, repoPath, cloneOptions);
    }

    protected void CreateBranchWithCommit(Repository repository, string branchName, string filePath, string content,
        string commitMessage = "Initial commit")
    {
        var branch = repository.CreateBranch(branchName);
        Commands.Checkout(repository, branchName);
        File.AppendAllText(filePath, content);
        Commands.Stage(repository, filePath);
        repository.Commit(commitMessage, new Signature(Identity, DateTimeOffset.Now),
            new Signature(Identity, DateTimeOffset.Now));

        var remote = repository.Network.Remotes["origin"];
        repository.Branches.Update(branch, b => b.Remote = remote.Name,
            b => b.UpstreamBranch = branch.CanonicalName);

        repository.Network.Push(branch, PushOptions);
    }


    protected void ClearRepository(Repository repository, bool clearRemote = true)
    {
        Commands.Checkout(repository, "master");

        var branches = repository.Branches.ToArray();

        foreach (var branch in branches)
        {
            if (branch.FriendlyName == "master")
            {
                continue;
            }

            if (!branch.IsRemote)
                repository.Branches.Remove(branch);

            if (clearRemote)
                repository.Network.Push(repository.Network.Remotes["origin"],
                    $":{branch.CanonicalName}", PushOptions);
        }

        repository.Dispose();
    }

    public abstract void Dispose();
}