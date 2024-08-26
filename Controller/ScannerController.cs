using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
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
        var data = await WiaService.GetData();
        ViewBag.Scanner = data ?? new List<string>(){};
        ViewBag.isDuplex = false;
        ViewBag.isFeeder = false;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Index(string scanner, string format, bool isDuplex, bool isFeeder, bool isAuto)
    {
        _logger.LogInformation(
            $"Переданы следующие настройки - Scanner: {scanner}, format: {format}, isDuplex: {isDuplex}, isFeeder: {isFeeder}, isAuto: {isAuto}");

        var sc = new Scanner()
        {
            Name = scanner,
            isDuplex = isDuplex,
            IsFeeder = isFeeder,
            IsAuto = isAuto,
            Format = format
        };

        await WiaService.Scan(sc);

        return Redirect("~/Scanner");
    }
}