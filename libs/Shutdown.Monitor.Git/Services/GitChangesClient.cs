using LibGit2Sharp;
using Shutdown.Monitor.Git.Common.Configs;

namespace Shutdown.Monitor.Git.Services;

public abstract class GitChangesClient : IDisposable
{
    protected readonly Repository Repository;
    protected readonly GitConfig Configuration;
    protected readonly PushOptions PushOptions;
    protected readonly Identity Identity;

    protected GitChangesClient(GitConfig configuration)
    {
        Repository = new Repository(configuration.RepoPath);
        Configuration = configuration;
        PushOptions = new PushOptions
        {
            CredentialsProvider = (url, usernameFromUrl, types) =>
                new UsernamePasswordCredentials()
                {
                    Username = Configuration.UserName,
                    Password = Configuration.Password
                }
        };
        Identity = new Identity(Configuration.Name, Configuration.Email);
    }

    public void Dispose()
    {
        Repository.Dispose();
    }
}