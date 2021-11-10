using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using System.Threading.Tasks;
using Utils;

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("")]
    public class HandlesController : ControllerBase
    {
        private readonly ILogger<HandlesController> _logger;
        private const string runningMessage = "File uploaded successfully.";

        private readonly CommonsUtilst commonsUtilst;

        public HandlesController(ILogger<HandlesController> logger)
        {
            _logger = logger;
            commonsUtilst = new CommonsUtilst(_logger);
        }

        [HttpGet]
        public string GetParam([FromQuery] MessageResponse messageResponse)
        {
            //Create url Naumen from request js form
            // method show params js request commonsUtilst.printConsoleAndLogFile("messageResponse", messageResponse);
            string urlToRequestNaumen = commonsUtilst.createUrl(messageResponse); ;

            // string pathTempDir = Environment.GetEnvironmentVariable("TMPDIR") + messageResponse.File;
            string pathTempDir = "C:\\Windows\\Temp\\" + messageResponse.File;

            // wait save to local storage
            Thread.Sleep(2000);

            byte[] resultBuff = commonsUtilst.getByteLocalFile(pathTempDir);

            if (resultBuff.Length > 0)
            {
                Task<string> task = commonsUtilst.Upload(resultBuff, urlToRequestNaumen, messageResponse, pathTempDir);
                commonsUtilst.printConsoleAndLogFile("Response Naumen:", task.Result);
                //log

                //TODO
                // PrintLog();

                commonsUtilst.PrintLog(messageResponse, resultBuff);

                return runningMessage;
            }
            return "File not exist to path: " + pathTempDir;
        }
    }
}
