using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.IO
{
    class TextFileHandler
    {
        
        
        public static async Task<String> ParseTextFile(string filepath) {

            try {
                using (StreamReader file = new StreamReader(filepath)) {

                    return await file.ReadToEndAsync();
                }
            }
            catch(Exception e) {
                throw;
            }
        }


    }
}
