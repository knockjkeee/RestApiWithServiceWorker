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
            commonsUtilst.printConsoleAndLogFile("messageResponse", messageResponse);

            //Create url Naumen from request ja form
            string urlToRequestNaumen = commonsUtilst.createUrl(messageResponse); ;

            // string pathTempDir = Environment.GetEnvironmentVariable("TMPDIR") + messageResponse.File;
            string pathTempDir = "C:\\Windows\\Temp\\" + messageResponse.File;

            byte[] resultBuff = commonsUtilst.getByteLocalFile(pathTempDir);
            if (resultBuff.Length > 0)
            {
                Task<string> task = commonsUtilst.Upload(resultBuff, urlToRequestNaumen, messageResponse, pathTempDir);
                System.Console.WriteLine(task.Result);
                return runningMessage;
            }

            return "File not exist to path: " + pathTempDir;
        }

    }
}
