using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Service;

namespace RestApiWithServiceWorker.Controller;

[Route("")]
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
        DataStore.SetData(messageResponse);
        
        var data = await WiaService.GetData();
        ViewBag.Scanner = data ?? new List<string>() { };
        ViewBag.isDuplex = false;
        ViewBag.isFeeder = false;
        ViewBag.FromQuery = null;
        // if (!messageResponse.IsValid)
        // {
        //     return View();
        // }

        // ViewBag.FromQuery = messageResponse;
        // ViewBag.IsValid = messageResponse.IsValid;
        // ViewBag.Rest = messageResponse.Rest;
        // ViewBag.AccessKey = messageResponse.AccessKey;
        // ViewBag.Uuid = messageResponse.Uuid;
        // ViewBag.Url = messageResponse.Url;
        // ViewBag.Attr = messageResponse.Attr;

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Index([FromForm] IndexDTO indexDto)
    {
        _logger.LogInformation(
            $"Переданы следующие настройки - Scanner: {indexDto.Scanner}, format: {indexDto.Format}, isDuplex: {indexDto.IsDuplex}, isFeeder: {indexDto.IsFeeder}");

        var sc = new Scanner()
        {
            Name = indexDto.Scanner,
            isDuplex = indexDto.IsDuplex,
            IsFeeder = indexDto.IsFeeder,
            Format = indexDto.Format,
            messageResponse = DataStore.GetData(),
            isDebug = string.IsNullOrEmpty(indexDto.Scanner)
        };
        DataStore.SetData(null);

        var bRes = await WiaService.Scan(sc);

        sc.messageResponse.File = sc.File;
        sc.messageResponse.Fname = sc.File;
        
        if(bRes)
            bRes = sc.messageResponse.IsValid && await SendFileToNaumen.SendData(sc.messageResponse);
        
        if(!bRes)
            _logger.LogError($"Ошибка в передаче данных в Naumen,  sc - {0}", sc);
        
        return Redirect("~/Scanner");
    }
}