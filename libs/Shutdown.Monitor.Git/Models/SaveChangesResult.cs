namespace Shutdown.Monitor.Git.Models;

public record SaveChangesResult(string TempBranchName, string LastBranchName, IEnumerable<CommitedFile> CommitedFiles);