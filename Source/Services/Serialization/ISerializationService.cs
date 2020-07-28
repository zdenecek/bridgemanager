using BridgeManager.Source.Model;

namespace BridgeManager.Source.Services
{
    public interface ISerializationService
    {
        Tournament Deserialize(string filepath);
        void Serialize(Tournament tournament, string filename);
    }
}