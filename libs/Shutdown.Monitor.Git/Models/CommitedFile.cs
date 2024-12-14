namespace Shutdown.Monitor.Git.Models;

public class CommitedFile
{
    public CommitedFile(string path, bool wasUntracked)
    {
        Path = path;
        WasUntracked = wasUntracked;
    }

    public string Path { get; init; }
    public bool WasUntracked { get; init; }
}