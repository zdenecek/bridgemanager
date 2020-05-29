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

namespace BridgeManager.Source.IO
{
    public class MovementFileParser
    {
        public enum MovementFileFormat { NotSet, Unknown, GiborgFile }


        public static async Task<Movement> ParseMovementFromFile(string filepath) {

            MovementFileFormat format = MovementFileFormat.NotSet;

            string loaded = "";

            try {
                loaded = await TextFileHandler.ParseTextFile(filepath);
            }
            catch (Exception) { throw; }

            //GIBORG FILE
            if (Path.GetExtension(filepath) == ".DAT" &&
                loaded.Contains("====================================")) format=MovementFileFormat.GiborgFile;
            else throw new ArgumentException("Unknown tournament file format.");


            switch (format) {
                case MovementFileFormat.GiborgFile:
                    return await ParseGiborgFile(loaded);
                default: return null;
            }
        }

        private static async Task<Movement> ParseGiborgFile(String fileData) {
            try {
                string[] lines = fileData.Split('\n');
                string name = lines[2].Split(',')[0];
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

               Console.WriteLine(countingRounds);

                for (int i = 0; i < lines.Length; i++) {

                    if (lines[i].StartsWith("===")) {
                        if (dataLines) {
                            if (countingRounds) {
                                rounds = Int32.Parse(lines[i - 1].Split('|')[0].Trim());
                                tables = (lines.Length - 1) / (17 + rounds);
                                data = new RoundTableData[tables, rounds];
                                countingRounds = false;
                                i -= rounds;
                                Console.WriteLine($"tables:{tables}, rounds:{rounds}");
                                continue;
                            }
                            table++;                   
                        }
                        dataLines = !dataLines;

                    } else if (dataLines && !countingRounds) {
                        string[] line = Regex.Replace(lines[i], @"\s+", " ").Split('|');
                        string[] pairs = line[1].TrimEnd().TrimStart().Split(' ');
                        string[] boards = line[2].Trim().Split('-');

                        curr = new RoundTableData();
                        curr.pairNS = (pairs[0] == "--") ? -1 : Int32.Parse(pairs[0]);
                        curr.pairEW = (pairs[1] == "--") ? -1 : Int32.Parse(pairs[1]);
                        curr.boardsLow = Int32.Parse(boards[0]);
                        curr.boardsHigh = Int32.Parse(boards[1]);
                        data[table - 1, Int32.Parse(line[0].Trim()) - 1] = curr;

                        Console.WriteLine($"Saving TableRoundData of NS:{pairs[0]} EW:{pairs[1]} Boards:{boards[0]}-{boards[1]} to indexes [{table - 1},{ Int32.Parse(line[0].Trim()) - 1}]");
                    }
                }
                return new Movement(data, name, description);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return new Movement(null,null,null);
            } 
        }
    
    }
}
