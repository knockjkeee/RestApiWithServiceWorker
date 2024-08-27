using System.Collections.Generic;
using System.Threading.Tasks;
using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Service;

public interface IWiaService
{
    Task<List<string>> GetData();
 
    Task<bool> Scan(Scanner scanner);
}

public class WiaService : IWiaService
{
    private IWiaDevices WiaDevice { get; set; }
    
    public WiaService(IWiaDevices scanner)
    {
        WiaDevice = scanner;
    }
    
    
    public async Task<List<string>> GetData()
    {
        return await WiaDevice.GetWiaDevices();
    }

    public async Task<bool> Scan(Scanner scanner)
    {
        return await WiaDevice.Scan(scanner);
    }
}