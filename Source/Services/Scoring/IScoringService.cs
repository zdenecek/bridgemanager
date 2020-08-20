using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Scoring
{
    public interface IScoringService
    {
        void CreateIntermediateSessionScores(Session session);
        void CreateSessionScores(Session session);
        void CreateCrossTable(Session session);
    }       
}