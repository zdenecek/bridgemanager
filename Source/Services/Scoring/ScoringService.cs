using BridgeManager.Source.Model;
using BridgeManager.Source.Model.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services.Scoring
{
    public class ScoringService : IScoringService
    {
        private IScorer scorer;

        public ScoringService()
        {
            scorer = new MPScorer();
        }

        public void CreateCrossTable(Session session)
        {
            try
            {
                scorer.CreateCrossTable(session);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public void CreateIntermediateSessionScores(Session session)
        {
            try
            {
                scorer.CreateIntermediateScores(session);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public void CreateSessionScores(Session session)
        {
            try
            {
                scorer.CreateSessionScores(session);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

    }
}
