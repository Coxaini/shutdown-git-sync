using Shutdown.Monitor.Git.Common;
using Shutdown.Monitor.Git.Common.Exceptions;

namespace Shutdown.Monitor.Git.Exceptions;

public class NoChangesInRepository : GitException
{
    public NoChangesInRepository() : base("No changes in repository")
    {
    }
}