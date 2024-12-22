using LibGit2Sharp;

namespace Shutdown.Monitor.Git.Common.Helpers;

public static class BranchExtenstion
{
    public static string GetLocalFriendlyNameFromRemoteBranch(this Branch branch)
    {
        return branch.FriendlyName.Replace($"{branch.RemoteName}/", "");
    }
}