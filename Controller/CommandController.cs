using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkerService;

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("Echo/{apiCommand}")]
    public class CommandController : ControllerBase
    {
        
        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }

        private string RunningMessage() => $"Echo: {Worker.ApiCommand}";

        [HttpGet]
        public string SetCommand(string apiCommand)
        {
            Worker.ApiCommand = apiCommand;
            _logger.LogInformation(RunningMessage());
            return RunningMessage();
        }
    }
}