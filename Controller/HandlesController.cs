using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Service;
using RestApiWithServiceWorker.Utils;

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("")]
    public class HandlesController : ControllerBase
    {
        private readonly ILogger<HandlesController> _logger;
        private readonly ISendFileToNaumen sendFileToNaumen;
        private const string runningMessage = "File uploaded successfully.";

        private readonly CommonsUtilst commonsUtilst;

        public HandlesController(ILogger<HandlesController> logger, ISendFileToNaumen sendFileToNaumen)
        {
            _logger = logger;
            this.sendFileToNaumen = sendFileToNaumen;
            commonsUtilst = new CommonsUtilst(_logger);
        }

        [HttpGet]
        public async Task<string> GetParam([FromQuery] MessageResponse messageResponse)
        {
            return await sendFileToNaumen.SendData(messageResponse, runningMessage);
            // return FileNotExistToPath(messageResponse, runningMessage);
            
        }

        // private string FileNotExistToPath(MessageResponse messageResponse, string message)
        // {
        //     var urlToRequestNaumen = commonsUtilst.CreateUrl(messageResponse); ;
        //
        //     // string pathTempDir = Environment.GetEnvironmentVariable("TMPDIR") + messageResponse.File;
        //     var pathTempDir = "C:\\Windows\\Temp\\" + messageResponse.File;
        //     // var pathTempDir = Path.GetTempPath() + messageResponse.File; //todo new folder
        //
        //     // wait save to local storage
        //     Thread.Sleep(2000); //todo remove this
        //
        //     var resultBuff = commonsUtilst.GetByteLocalFile(pathTempDir);
        //
        //     if (resultBuff.Length <= 0) return "File not exist to path: " + pathTempDir;
        //     
        //     var task = commonsUtilst.Upload(resultBuff, urlToRequestNaumen, messageResponse, pathTempDir);
        //     commonsUtilst.PrintConsoleAndLogFile("Response Naumen:", task.Result);
        //     commonsUtilst.PrintLog(messageResponse, resultBuff);
        //     return message;
        // }
    }
}
