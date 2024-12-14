namespace Shutdown.Monitor.Git.Models;

public record SaveChangesResult(string BranchName, IEnumerable<CommitedFile> CommitedFiles);