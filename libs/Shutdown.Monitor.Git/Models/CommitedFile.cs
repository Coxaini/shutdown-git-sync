namespace Shutdown.Monitor.Git.Models;

public class CommitedFile
{
    public CommitedFile(string filePath, bool wasUntracked)
    {
        FilePath = filePath;
        WasUntracked = wasUntracked;
    }

    public string FilePath { get; init; }
    public bool WasUntracked { get; init; }
}