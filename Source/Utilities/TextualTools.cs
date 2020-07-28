using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services {


    public static class TextualTools {

        public static string GetLetter(int number) {

            return number == 0 ? "?" : ((char)(number + 64)).ToString();
        }
    }
}
