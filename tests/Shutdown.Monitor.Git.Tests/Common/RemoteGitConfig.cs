namespace Shutdown.Monitor.Git.Tests.Common;

public class RemoteGitConfig
{
    public string Origin { get; init; } = null!;
    public string TempBranchPrefix { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}