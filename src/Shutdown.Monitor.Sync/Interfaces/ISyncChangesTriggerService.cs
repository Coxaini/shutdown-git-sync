using Shutdown.Monitor.Sync.Models;

namespace Shutdown.Monitor.Sync.Interfaces;

public interface ISyncChangesTriggerService
{
    void SyncChanges(SyncRequest syncRequest);
}