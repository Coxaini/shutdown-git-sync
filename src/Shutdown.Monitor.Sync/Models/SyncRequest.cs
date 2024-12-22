namespace Shutdown.Monitor.Sync.Models;

public record SyncRequest(string BranchName, string TempBranchName, IEnumerable<SyncFile> Files);