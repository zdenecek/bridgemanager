using BridgeManager.Source.Model;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services.Movements
{
    public interface IMovementsService
    {
        Task<Movement> ParseMovementFromFile(string loadedText, string filepath);
    }
}