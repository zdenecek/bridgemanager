using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model.Scoring {
    public class MPScorer : IScorer        
        {

        /*
         *  Add 1 to the number of match points scored. (If the North American match point system is in use, where each comparison is worth one point rather than two, add a half-point instead.)
            Multiply by the number of times the board should have been played (this should be the same number for all the boards in the tournament) and divide by the number of times it was actually played.
            Then subtract 1 (or ½, whichever was added above). 
        
         */


        public void CreatePartialScores(Session s) {

            ObservableCollection<PartialScore> partialScores = new ObservableCollection<PartialScore>();

            s.PartialScores = partialScores;

            foreach (Section section in s.Sections) {

                var boards =
                    from result in s.Results
                    where result.Section.Equals(section)
                    group result by result.Board into boardGroup
                    from scoreGroup in
                        (from result in boardGroup
                         group result by result.NSPoints())
                    group scoreGroup by boardGroup.Key;

                int supposedNumberOfBoardPlays = boards.Select(boardsGroup => boardsGroup.Count()).Max();

                foreach (var boardGroup in boards) {
                    foreach(var resultsGroup in boardGroup) {
                        
                    }  
                }
            }  
        }

        private float NeubergAdjust(int plays, int intendedPlays, int MPs) {
            MPs++;
            return MPs * intendedPlays/ (float)plays;
        }
    }
}
