using Shutdown.Monitor.Git.Models;
using Shutdown.Monitor.Sync.Models;

namespace Shutdown.Monitor.Sync.Interfaces;

public interface ITriggerSyncHttpClient
{
    Task TriggerSync(SyncRequest request);
}