using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Service;

public interface IDataStore
{
    void SetMR(MessageResponse data);
    MessageResponse GetMR();

    void SetSc(Scanner sc);

    Scanner GetSc();

}

public class DataStore : IDataStore
{
    private MessageResponse Data { get; set; }

    private Scanner Scanner { get; set; }

    public void SetMR(MessageResponse data)
    {
        Data = data;
    }

    public MessageResponse GetMR()
    {
        return Data;
    }
    
    
    public void SetSc(Scanner sc)
    {
        Scanner = sc;
    }

    public Scanner GetSc()
    {
        return Scanner;
    }
    
}