using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Configuration;

namespace Shutdown.Monitor.Git.Tests.Common;

public abstract class GitTestBase : IDisposable
{
    protected readonly RemoteGitConfig RemoteGitConfig;
    protected readonly Repository Repository;
    protected readonly PushOptions PushOptions;
    protected readonly PullOptions PullOptions;
    protected readonly Identity Identity;

    protected GitTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .AddUserSecrets<GitTestBase>()
            .Build();

        var config = configuration.Get<RemoteGitConfig>();

        RemoteGitConfig = config ?? throw new ArgumentNullException(nameof(RemoteGitConfig));

        Repository = new Repository(RemoteGitConfig.RepoPath);
        CredentialsHandler credentialsHandler = (url, usernameFromUrl, types) =>
            new UsernamePasswordCredentials
            {
                Username = RemoteGitConfig.UserName,
                Password = RemoteGitConfig.Password
            };

        PushOptions = new PushOptions
        {
            CredentialsProvider = credentialsHandler
        };
        PullOptions = new PullOptions()
        {
            FetchOptions = new FetchOptions()
            {
                CredentialsProvider = credentialsHandler
            }
        };

        Identity = new Identity(RemoteGitConfig.Name, RemoteGitConfig.Email);
    }

    protected void CreateTestBranchWithCommit(string branchName, string fileName, string content)
    {
        var branch = Repository.CreateBranch(branchName);
        Commands.Checkout(Repository, branchName);
        var file = Path.Combine(RemoteGitConfig.RepoPath, fileName);
        File.WriteAllText(file, content);
        Commands.Stage(Repository, file);
        Repository.Commit("Initial commit", new Signature(Identity, DateTimeOffset.Now),
            new Signature(Identity, DateTimeOffset.Now));

        var remote = Repository.Network.Remotes["origin"];
        Repository.Branches.Update(branch, b => b.Remote = remote.Name,
            b => b.UpstreamBranch = branch.CanonicalName);

        Repository.Network.Push(branch, PushOptions);
    }

    private void ClearRepository()
    {
        Commands.Checkout(Repository, "master");

        var branches = Repository.Branches.ToArray();

        foreach (var branch in branches)
        {
            if (branch.FriendlyName == "master")
            {
                continue;
            }

            if (!branch.IsRemote)
                Repository.Branches.Remove(branch);

            Repository.Network.Push(Repository.Network.Remotes["origin"],
                $":{branch.CanonicalName}", PushOptions);
        }
    }

    public void Dispose()
    {
        ClearRepository();
    }
}