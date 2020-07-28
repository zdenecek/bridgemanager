using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services.Movements.Parsers {
    public class GiborgParser : IMovementParser {

        public string ParserName { get; private set; }

        public GiborgParser() {
            ParserName = "Giborg .DAT file parser";
        }

        public bool CanParse(string loadedText, string filepath) {
             return Path.GetExtension(filepath) == ".DAT" &&
                loadedText.Contains("====================================");
        }

        public Movement Parse(string loadedText, string filePath) {
            try {
                string[] lines = loadedText.Split('\n');
                string name = lines[1].Trim();
                string description = "Giborg.DAT file";

                //algorithm state variables
                bool dataLines = false;
                int table = 1;
                bool countingRounds = true;

                //movement variables
                int rounds = 0;
                /* rows = 3 + (17+rounds)tables - 2
                 * (rows - 1)/(17 + rounds) = tables
                 */
                int tables;
                RoundTableData[,] data = { };
                RoundTableData curr;

                for (int i = 0; i < lines.Length; i++) {

                    if (lines[i].StartsWith("===")) {
                        if (dataLines) {
                            if (countingRounds) {
                                rounds = Int32.Parse(lines[i - 1].Split('|')[0].Trim());
                                tables = (lines.Length - 1) / (17 + rounds);
                                data = new RoundTableData[tables, rounds];
                                countingRounds = false;
                                i -= rounds+1;
                               // Console.WriteLine($"tables:{tables}, rounds:{rounds}");
                                continue;
                            }
                            table++;
                        }
                        dataLines = !dataLines;

                    }
                    else if (dataLines && !countingRounds) {
                        string[] line = Regex.Replace(lines[i], @"\s+", " ").Split('|');
                        string[] pairs = line[1].TrimEnd().TrimStart().Split(' ');
                        string[] boards = line[2].Trim().Split('-');

                        curr = new RoundTableData() {
                            pairNS = (pairs[0] == "--") ? -1 : Int32.Parse(pairs[0]),
                            pairEW = (pairs[1] == "--") ? -1 : Int32.Parse(pairs[1]),
                            boardsLow = Int32.Parse(boards[0]),
                            boardsHigh = Int32.Parse(boards[1]),
                            table = table,
                            round = Int32.Parse(line[0].Trim())
                        };
                        data[curr.table - 1, curr.round - 1] = curr;

                        //Console.WriteLine($"Saving TableRoundData of NS:{pairs[0]} EW:{pairs[1]} Boards:{boards[0]}-{boards[1]} to indexes [{table - 1},{ Int32.Parse(line[0].Trim()) - 1}]");
                    }
                }
                
                return new Movement(0, data, name, description);
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
