namespace Shutdown.Monitor.Git.Common.Exceptions;

public class GitException : Exception
{
    public GitException(string message) : base(message)
    {
    }
}