using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkerService;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("Command/{apiCommand}")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private string RunningMessage() => $"apiCommand: {Worker.ApiCommand}";

        public CommandController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string SetCommand(string apiCommand)
        {
            Worker.ApiCommand = apiCommand;
            _logger.LogInformation(RunningMessage());
            //Console.WriteLine(RunningMessage());
            
            return RunningMessage();
        }
    }
}
