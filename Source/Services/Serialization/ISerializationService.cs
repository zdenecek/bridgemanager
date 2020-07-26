using BridgeManager.Source.Model;

namespace BridgeManager.Source.IO
{
    public interface ISerializationService
    {
        Tournament Deserialize(string filepath);
        void Serialize(Tournament tournament, string filename);
    }
}