using BridgeManager.Source.IO.MovementParsing;
using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BridgeManager.Source.IO.MovementParsing
{
    public class MovementFileParser {

        public List<IMovementParser> Parsers { get; private set; }

        public MovementFileParser() {
            Parsers = new List<IMovementParser> {
                new GiborgParser()
            };
        }

        public async Task<Movement> ParseMovementFromFile(string loadedText, string filepath) {

            IMovementParser selectedParser = null;

            try {
                foreach(IMovementParser parser in Parsers) {
                    if (parser.CanParse(loadedText, filepath))
                        selectedParser = parser;
                }

                if (selectedParser == null) {
                    Console.WriteLine("Unknown movement file type");
                    return null;
                }

                var m = selectedParser.Parse(loadedText, filepath);

                Console.WriteLine($"Movement parsed succesfully. Name: {m.Name} Description: {m.Description}");
                return m;

            }
            catch (Exception e) {
                Console.WriteLine($"Error parsing movement. File: {filepath} Used parser {selectedParser?.ParserName}");
                return null;
            }
        }    
    }
}
