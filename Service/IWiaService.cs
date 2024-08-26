using System.Collections.Generic;
using System.Threading.Tasks;
using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Service;

public interface IWiaService
{
    Task<List<string>> GetData();

    Task Scan(Scanner scanner);
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

    public async Task Scan(Scanner scanner)
    {
        await WiaDevice.Scan(scanner);
    }
}