using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source {


    public static class Local {


        private static readonly Dictionary<string, string> strings = new Dictionary<string, string>();
        

        public static void Initialize() {

            ///TEMPORARY
            strings.Add("unknown_player", "unknown");
        }

        public static string Get(string key) {
            string value;
            strings.TryGetValue(key, out value);
            return value;
        }

        public static string GetLetter(int number) {

            return number == 0 ? "?" : ((char)(number + 64)).ToString();
        }
    }
}
