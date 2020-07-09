using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public interface IScorer {

        void CreatePartialScores(Session s);

    }
}
