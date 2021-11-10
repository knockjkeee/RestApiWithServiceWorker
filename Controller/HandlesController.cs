using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using System.Text.RegularExpressions;
using Utils;
using Microsoft.AspNetCore.Http;

// using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("")]
    public class HandlesController : ControllerBase
    {
        private readonly ILogger<HandlesController> _logger;
        private const string runningMessage = "WindowsServiceApi is running...";

        private readonly CommonsUtilst commonsUtilst;

        public HandlesController(ILogger<HandlesController> logger)
        {
            _logger = logger;
            commonsUtilst = new CommonsUtilst(_logger);
        }

        [HttpGet]
        public string GetParams([FromQuery] MessageResponse messageResponse)
        {
            commonsUtilst.printConsoleAndLogFile("messageResponse", messageResponse);

            string urlToRequestNaumen = commonsUtilst.createUrl(messageResponse); ;
            commonsUtilst.printConsoleAndLogFile("urlToRequestNaumen", urlToRequestNaumen);

            string pathTempDir = Environment.GetEnvironmentVariable("TMPDIR") + messageResponse.File;
            commonsUtilst.printConsoleAndLogFile("Path temp dir", pathTempDir);

            byte[] resultBuff = commonsUtilst.getByteLocalFile(pathTempDir);
            commonsUtilst.printConsoleAndLogFile("buff length", resultBuff.Length);


            // Task<string> task = Upload(buff);

            // await Response.WriteAsync(runningMessage);
            return runningMessage;
        }

        public static async Task<string> Upload(byte[] image)
        {
            using (var client = new HttpClient())
            {
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new MemoryStream(image)), "bilddatei", "upload.jpg");

                    using (
                       var message =
                           await client.PostAsync("http://www.directupload.net/index.php?mode=upload", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;
                    }
                }
            }
        }
    }
}
