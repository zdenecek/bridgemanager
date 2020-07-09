using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.IO {
    public static class BridgeMath {

        public static int GetNSScore(Result result) {
            if (result.NotPlayedOrArbitraryScore()) throw new Exception("Cannot return NSPoints, there was no play or an arbitrary score");
            else if (result.Contract == "PASS") return 0;

            var c = result.Contract.Split(' ');

            ///CONTRACT SETTINGS
            bool NSdecl = result.Declarer.Equals(result.PairNS);
            bool vul = BridgeMath.IsVulnerable(result.Board, NSdecl);
            int level = Int32.Parse(c[0]);
            string suit = c[1];
            int doubled = 0; // 0 normal 1 double 2 redoubled 
            if (c.Length == 3) {
                if (c[2] == "x") doubled = 1;
                else if (c[2] == "xx") doubled = 2;
            }

            int score = 0;
            if (result._Result.StartsWith("-")) return Undertricks(-Int16.Parse(result._Result), vul, doubled);
            else if (result._Result.StartsWith("+")) score += Overtricks(Int32.Parse(result._Result), suit, vul, doubled);

            score += Made(level, suit, vul, doubled);

            return NSdecl ? score : -score;
        }

        public static bool IsVulnerable(int BoardNumber, bool isNS) {
            return isNS ? nsVul[BoardNumber % 16] : ewVul[BoardNumber % 16];
        }

        public static int Overtricks(int count, string suit, bool vul, int state) {
            if (state > 0) return count  * (vul ? 200 : 100) * state; //x1 when doubled x2 when redoubled
            else return count * (suit == "C" || suit == "D" ? 20 : 30);
        }

        public static int Made(int level, string suit, bool vul, int state) {
            int score = 0;
            score += level * (suit == "C" || suit == "D" ? 20 : 30);
            if (suit == "NT") score += 10;

            if (state > 0) score *= 2 * state; //2x when doubled 4x when redoubled

            bool isGame = score >= 100;
            bool isSmallSlam = level == 6;
            bool isGrandSlam = level == 7;

            if (isGrandSlam) score += vul ? 1500 : 1000;
            else if (isSmallSlam) score += vul ? 750 : 500;
            else if (!isGame) score += 50;

            if (isGame) score += vul ? 500 : 300;

            score += state * 50; //+0 normal +50 double +100 redouble

            return score;
        }

        public static int Undertricks(int falls, bool isVul, int doubled) {
            int score = 0;
            if (doubled > 0) {
                switch (falls) {
                    case 1:
                        score = isVul ? -200 : -100;
                        break;
                    case 2:
                        score = isVul ? -500 : -300;
                        break;

                    default:
                        score = (isVul ? -800 : -500) - ((falls - 3) * 300);
                        break;
                }
                return score * doubled; //1x when doubled, 2x when redoubled
            }
            else {
                return falls * (isVul ? -100 : -50);
            }
        }


        private static bool[] nsVul = new bool[] {
                                            false,                          //16
                                            false, true, false, true,       //1-4
                                            true, false, true, false,       //5-8
                                            false, true, false, true,       //9-12
                                            true, false, true };           //13-15
        private static bool[] ewVul = new bool[] {
                                            true,
                                            false, false, true, true,
                                            false, true, true, false,
                                            true, true, false, false,
                                            true, false, false};


    }
}
