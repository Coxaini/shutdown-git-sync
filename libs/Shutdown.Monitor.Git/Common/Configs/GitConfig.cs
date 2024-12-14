using Shutdown.Monitor.Git.Common.Constants;

namespace Shutdown.Monitor.Git.Common.Configs;

public class GitConfig
{
    public required string RepoPath { get; set; }
    public required string Origin { get; set; }
    public string TempBranchPrefix { get; set; } = GitConstants.TempBranchPrefix;
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public string TempCommitMessage { get; set; } = GitConstants.TempCommitMessage;
    public string Email { get; init; } = null!;
    public string Name { get; init; } = null!;
}