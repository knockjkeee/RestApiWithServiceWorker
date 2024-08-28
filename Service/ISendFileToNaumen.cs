using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Utils;

namespace RestApiWithServiceWorker.Service;

public interface ISendFileToNaumen
{
    Task<string> SendData(MessageResponse messageResponse, string message);
    Task<bool> SendData(MessageResponse messageResponse);
}

public class SendFileToNaumen : ISendFileToNaumen
{
    private readonly ILogger<SendFileToNaumen> _logger;
    private readonly CommonsUtilst commonsUtils;

    public SendFileToNaumen(ILogger<SendFileToNaumen> logger)
    {
        this.commonsUtils = new CommonsUtilst(logger);
        _logger = logger;
    }


    public async Task<string> SendData(MessageResponse messageResponse, string message)
    {
        var urlToRequestNaumen = commonsUtils.CreateUrl(messageResponse);

        // string pathTempDir = Environment.GetEnvironmentVariable("TMPDIR") + messageResponse.File;
        var pathTempDir = "C:\\Windows\\Temp\\" + messageResponse.File;
        // var pathTempDir = Path.GetTempPath() + messageResponse.File; //todo new folder
        
        var resultBuff = commonsUtils.GetByteLocalFile(pathTempDir);
        if (resultBuff.Length <= 0) return "File not exist to path: " + pathTempDir;

        var task = await commonsUtils.Upload(resultBuff, urlToRequestNaumen, messageResponse, pathTempDir);
        commonsUtils.PrintConsoleAndLogFile("Response Naumen:", task);
        commonsUtils.PrintLog(messageResponse, resultBuff);
        return message;
    }
    
    public async Task<bool> SendData(MessageResponse messageResponse)
    {
        _logger.LogInformation("Подготовка к передачи в Naumen...");
        
        var urlToRequestNaumen = commonsUtils.CreateUrl(messageResponse);

        var pathTempDir = Path.GetTempPath() + messageResponse.File; //todo new folder
        
        var resultBuff = commonsUtils.GetByteLocalFile(pathTempDir);
        
        if (resultBuff.Length <= 0)
        {
            _logger.LogError("File not exist to path: " + pathTempDir);
            return false;
        }

        string task;
        
        try
        {
            task = await commonsUtils.Upload(resultBuff, urlToRequestNaumen, messageResponse, pathTempDir);
        }
        catch (Exception e)
        {
            _logger.LogError($"Ошибка в передаче данных в Naumen - {e}");
            return false;
        }
        
        File.Delete(pathTempDir);
        
        commonsUtils.PrintConsoleAndLogFile("Response Naumen:", task);
        commonsUtils.PrintLog(messageResponse, resultBuff);
        
        _logger.LogInformation("Передача в Naumen завершена...");
        return true;
    }
}