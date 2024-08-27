using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Service;

public interface IDataStore
{
    void SetData(MessageResponse data);
    MessageResponse GetData();
    
}

public class DataStore : IDataStore
{
    private MessageResponse Data { get; set; }
    
    public void SetData(MessageResponse data)
    {
        Data = data;
    }

    public MessageResponse GetData()
    {
        return Data;
    }
}