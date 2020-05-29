using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source
{
    public static class Tools
    {

        public static void printEventDetails(object sender, EventArgs args) {
            Console.WriteLine($"!event= sender: {sender.ToString()} args: {args.ToString()} ");
        }
    }
}
