using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services.Files
{
    public interface IFileService
    {
        Task<String> ParseTextFile(string filepath);
    }
}
