using BridgeManager.Source.Services.Database;

namespace BridgeManager.Source.Services.Database
{
    public interface IDatabaseService
    {
        
        

        void Open(string filePath);

        void Close();

        BMInput1DataSet Data { get; }
        
        void ClearInDB(BMTable table);
        void FetchFromDB(BMTable table);
        void SendToDB(BMTable table);
    }
}