using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkerService;

namespace RestApiWithServiceWorker.Controller
{
    [ApiController]
    [Route("Echo/{apiCommand}")]
    public class CommandController(ILogger<HandlesController> logger) : ControllerBase
    {
        private string RunningMessage() => $"Echo: {Worker.ApiCommand}";

        [HttpGet]
        public string SetCommand(string apiCommand)
        {
            Worker.ApiCommand = apiCommand;
            logger.LogInformation(RunningMessage());
            return RunningMessage();
        }
    }
}