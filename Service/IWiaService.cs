using System.Collections.Generic;
using System.Threading.Tasks;
using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Service;

public interface IWiaService
{
    Task<List<string>> GetData();
}

public class WiaService : IWiaService
{
    private IScanner Scanner { get; set; }
    
    public async Task<List<string>> GetData()
    {
        return await Scanner.GetWiaDevices();
    }
}