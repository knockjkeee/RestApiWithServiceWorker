using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Service;

namespace RestApiWithServiceWorker.Controller;

[Route("Scanner")]
public class ScannerController : Microsoft.AspNetCore.Mvc.Controller
{
    private IWiaService WiaService { get; set; }
    
    private readonly ILogger<ScannerController> _logger; 

    public ScannerController(IWiaService wiaService, ILogger<ScannerController> logger)
    {
        WiaService = wiaService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.People = await WiaService.GetData();
        ViewBag.isDuplex = true;
        ViewBag.isFeeder = true;
        ViewBag.isAuto = true;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Index(string scanner, bool isDuplex, bool isFeeder, bool isAuto)
    {
        // return $"Scanner is {scanner}\risDuplex: {isDuplex}\risFeeder: {isFeeder}\risAuto: {isAuto}";
        await Task.Delay(1);
        _logger.LogInformation($"Scanner is {scanner} isDuplex: {isDuplex} isFeeder: {isFeeder} isAuto: {isAuto}");
        return Redirect("~/Scanner");
    }
}