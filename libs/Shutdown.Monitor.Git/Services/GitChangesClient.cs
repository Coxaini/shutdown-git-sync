using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Shutdown.Monitor.Git.Common.Configs;

namespace Shutdown.Monitor.Git.Services;

public abstract class GitChangesClient : IDisposable
{
    protected readonly Repository Repository;
    protected readonly GitConfig Configuration;
    protected readonly PushOptions PushOptions;
    protected readonly FetchOptions FetchOptions;
    protected readonly PullOptions PullOptions;
    protected readonly Identity Identity;

    protected GitChangesClient(GitConfig configuration)
    {
        Repository = new Repository(configuration.RepoPath);
        Configuration = configuration;
        CredentialsHandler credentialsHandler = (url, usernameFromUrl, types) =>
            new UsernamePasswordCredentials()
            {
                Username = Configuration.UserName,
                Password = Configuration.Password
            };

        PushOptions = new PushOptions
        {
            CredentialsProvider = credentialsHandler
        };
        FetchOptions = new FetchOptions
        {
            CredentialsProvider = credentialsHandler
        };
        PullOptions = new PullOptions
        {
            FetchOptions = FetchOptions
        };

        Identity = new Identity(Configuration.Name, Configuration.Email);
    }

    public void Dispose()
    {
        Repository.Dispose();
    }
}