using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Utils;

namespace RestApiWithServiceWorker.Service;

public interface ISendFileToNaumen
{
    Task<string> SendData(MessageResponse messageResponse, string message);
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
}