using Shutdown.Monitor.Git.Common.Results;
using Shutdown.Monitor.Git.Models;

namespace Shutdown.Monitor.Git.Interfaces;

public interface IGitChangesSender
{
    GitResult<SaveChangesResult> SaveChangesToTempBranch();
}