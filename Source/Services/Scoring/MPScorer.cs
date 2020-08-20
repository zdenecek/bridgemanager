using BridgeManager.Source.Model;
using BridgeManager.Source.Model.Scoring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BridgeManager.Source.Services.Scoring
{
    public class MPScorer : IScorer
    {
        public void CreateCrossTable(Session session)
        {
            var sections = session.Sections.Count;
            var crossTable = new Score[sections][,];

            for(int i = 0; i < sections; i++)
            {
                var section = session.Sections.ElementAt(i);
                var pairs =  section.PairCount;
                var table = new Score[pairs, pairs];

                foreach(Score score in session.IntermediateScores.Where(s => s.AsociatedResult.Section.Equals(section))){
                    table[score.PlayerUnit.Number - 1, score.OpponentPlayerUnit.Number - 1] += score;
                }

                crossTable[i] = table;
            }
        }

        public void CreateIntermediateScores(Session session)
        {
            ObservableCollection<Score> scores = new ObservableCollection<Score>();

            session.IntermediateScores = scores;

            foreach (Section section in session.Sections)
            {
                if (session.Results.Count == 0) throw new InvalidOperationException("Cannot create intermediate scores, there are no board results.");

                var boards =
                    from result in session.Results
                    where result.Section.Equals(section) && result.NotPlayedOrArbitraryScore == false
                    group result by result.Board into board

                    from scoreGroup in
                        (from result in board
                         orderby result.NSPoints()
                         group result by result.NSPoints()
                         )
                    orderby board.Key
                    group scoreGroup by board.Key;

                //maximum number of plays is adjusted to the most played board, redo later.
                var supposedPlayCount = boards.Select(boardsGroup => boardsGroup.Count()).Max();
                var topMatchpoints = 2 * supposedPlayCount - 2;

                int boardPlayCount;
                int beatenPairs;
                int tiedPairs;

                int originalMPsForNS;
                decimal adjustedMPsForNS;
                decimal adjustedMPsForEW;

                foreach (var boardResults in boards)
                {
                    boardPlayCount = boardResults.Sum(b => b.Count());
                    beatenPairs = 0;

                    foreach (var resultGroup in boardResults)
                    {
                        tiedPairs = resultGroup.Count() - 1;
                        originalMPsForNS = beatenPairs * 2 + tiedPairs * 1;
                        adjustedMPsForNS = NeubergAdjust(boardPlayCount, supposedPlayCount, originalMPsForNS);
                        adjustedMPsForEW = topMatchpoints - adjustedMPsForNS;

                        foreach (var result in resultGroup)
                        {
                            scores.Add(new Score()
                            {
                                AsociatedResult = result,
                                PlayerUnit = result.PairNS,
                                OpponentPlayerUnit = result.PairEW,
                                MaxPoints = topMatchpoints,
                                Points = adjustedMPsForNS
                            });
                            scores.Add(new Score()
                            {
                                AsociatedResult = result,
                                PlayerUnit = result.PairEW,
                                OpponentPlayerUnit = result.PairNS,
                                MaxPoints = topMatchpoints,
                                Points = adjustedMPsForEW
                            });
                        }

                        beatenPairs += resultGroup.Count();
                    }
                }
            }
        }

        public void CreateSessionScores(Session session)
        {
            if (session.IntermediateScores.Count == 0) throw new InvalidOperationException("Cannot create session scores, intermediate scores are not present");

            ObservableCollection<Score> scores = new ObservableCollection<Score>();
            session.FinalScores = scores;

            var intermediate = from intermediateResult in session.IntermediateScores
                               group intermediateResult by intermediateResult.PlayerUnit;

            foreach(var pairGroup in intermediate)
            {
                var score = new Score()
                {
                    PlayerUnit = pairGroup.Key,
                    Points = 0,
                    MaxPoints = 0
                };

                foreach(Score intermediateScore in pairGroup)
                {
                    score.Points += intermediateScore.Points;
                    score.MaxPoints += intermediateScore.MaxPoints;
                }

                scores.Add(score);
            }
        }

        /*  Neuberg adjust as per Wikipedia
         * 
         *  Add 1 to the number of match points scored. (If the North American match point system is in use, where each comparison is worth one point rather than two, add a half-point instead.)
         *  Multiply by the number of times the board should have been played (this should be the same number for all the boards in the tournament) 
         *  and divide by the number of times it was actually played.
         *  Then subtract 1 (or ½, whichever was added above). 
         */

        private decimal NeubergAdjust(int plays, int intendedPlays, int MPs)
        {
            MPs++;
            return Math.Round( (MPs * intendedPlays / (decimal)plays) - 1, 1);
        }
    }
}
