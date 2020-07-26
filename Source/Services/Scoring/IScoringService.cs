using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Scoring
{
    public interface IScoringService
    {
        void CreateIntermediateScores(Session session);
        void CreateSessionScores(Session session);
    }       
}