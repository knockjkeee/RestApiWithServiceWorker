using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("Home")]
    // [EnableCors("AllowCors")]
    // [DisableCors]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private const string runningMessage = "WindowsServiceApi is running...";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // [DisableCors]
        [HttpGet]
        public string GetParams([FromQuery] MessageResponse messageResponse)
        {
            Console.WriteLine("messageResponse: " + messageResponse);

            _logger.LogInformation(runningMessage);
            _logger.LogInformation("messageResponse: " + messageResponse);

            // Console.WriteLine(runningMessage);

            return runningMessage;
        }
    }
}
