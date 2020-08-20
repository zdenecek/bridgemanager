using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Scoring
{
    public interface IScorer {

        void CreateIntermediateScores(Session session);

        void CreateSessionScores(Session session);

        void CreateCrossTable(Session session);

    }
}
