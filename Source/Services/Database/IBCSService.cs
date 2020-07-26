using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Database
{
    public interface IBCSService
    {
        void RetrieveResults(Session session, Tournament tournament);
        void SendMovementData(Session session);
    }
}