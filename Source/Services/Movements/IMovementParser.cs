using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.IO.MovementParsing {
    public interface IMovementParser {

        string ParserName { get; }

        bool CanParse(string loadedText, string filename);

        Movement Parse(string loadedText, string filename);

    }
}
