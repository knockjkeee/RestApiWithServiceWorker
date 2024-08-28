using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Service;

namespace RestApiWithServiceWorker.Controller;

[Route("")]
[Route("/TWAIN@Web")]
public class ScannerController : Microsoft.AspNetCore.Mvc.Controller
{
    private IWiaService WiaService { get; set; }
    private ISendFileToNaumen SendFileToNaumen { get; set; }
    private IDataStore DataStore { get; set; }

    private readonly ILogger<ScannerController> _logger;

    public ScannerController(IWiaService wiaService, ILogger<ScannerController> logger,
        ISendFileToNaumen sendFileToNaumen, IDataStore dataStore)
    {
        WiaService = wiaService;
        _logger = logger;
        SendFileToNaumen = sendFileToNaumen;
        DataStore = dataStore;
    }


    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] MessageResponse messageResponse)
    {
        var requestPath = HttpContext.Request.Path.Value;

        if (requestPath?.Contains("TWAIN") ?? default)
        {
            if (messageResponse.Hostname != null)
                messageResponse.Url = messageResponse.Hostname;
            if (messageResponse.Subject != null)
                messageResponse.Uuid = messageResponse.Subject;
        }

        messageResponse.QueryString = string.IsNullOrEmpty(HttpContext.Request.QueryString.Value)
            ? ""
            : HttpContext.Request.QueryString.Value;
        
        DataStore.SetMR(messageResponse);
        DataStore.SetSc(null);

        var data = await WiaService.GetData();
        ViewBag.Scanner = data ?? new List<string>() { };
        ViewBag.isDuplex = false;
        ViewBag.isFeeder = false;
        ViewBag.FromQuery = null;

        return View();
    }


    [HttpPost]
    public IActionResult Index([FromForm] IndexDTO indexDto)
    {
        _logger.LogInformation(
            $"Переданы следующие настройки - " +
            $"Scanner: {indexDto.Scanner}, " +
            $"format: {indexDto.Format}, " +
            $"isDuplex: {indexDto.IsDuplex}, " +
            $"isFeeder: {indexDto.IsFeeder}");

        var sc = new Scanner()
        {
            Name = indexDto.Scanner,
            isDuplex = indexDto.IsDuplex,
            IsFeeder = indexDto.IsFeeder,
            Format = indexDto.Format,
            messageResponse = DataStore.GetMR(),
            isDebug = string.IsNullOrEmpty(indexDto.Scanner)
        };
        
        DataStore.SetSc(sc);
        return Redirect($"~/Process");
    }
}