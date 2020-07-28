using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services.Files
{
    public class FileService : IFileService
    {
   
        public async Task<String> ParseTextFile(string filepath) {

            try {
                using (StreamReader file = new StreamReader(filepath)) {
                    return await file.ReadToEndAsync();
                }
            }
            catch(Exception) {
                throw;
            }
        }

    }
}
