using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Service;

namespace RestApiWithServiceWorker.Controller;

[Route("/Load")]
public class LoadController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ILogger<LoadController> _logger;

    private IWiaService WiaService { get; set; }
    private ISendFileToNaumen SendFileToNaumen { get; set; }
    private IDataStore DataStore { get; set; }

    public LoadController(IDataStore dataStore, ILogger<LoadController> logger, ISendFileToNaumen sendFileToNaumen,
        IWiaService wiaService)
    {
        DataStore = dataStore;
        _logger = logger;
        SendFileToNaumen = sendFileToNaumen;
        WiaService = wiaService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var sc = DataStore.GetSc();

        var bRes = sc.messageResponse.IsValid && await WiaService.Scan(sc);

        sc.messageResponse.File = sc.File;
        sc.messageResponse.Fname = sc.File;

        if (bRes)
            bRes = await SendFileToNaumen.SendData(sc.messageResponse);

        if (!bRes)
            _logger.LogError($"Ошибка в передаче данных в Naumen,  sc - {sc}");

        return Redirect($"~/{sc.messageResponse.QueryString}");
    }
}