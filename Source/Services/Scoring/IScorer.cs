using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Scoring
{
    public interface IScorer {

        void CreateIntermediateScores(Session s);

        void CreateSessionScores(Session s);

    }
}
