using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private const string runningMessage = "WindowsServiceApiDemo is running...";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation(runningMessage);

            Console.WriteLine(runningMessage);

            return runningMessage;
        }
    }
}
