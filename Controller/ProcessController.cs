using Microsoft.AspNetCore.Mvc;

namespace RestApiWithServiceWorker.Controller;

[Route("/Process")]
public class ProcessController : Microsoft.AspNetCore.Mvc.Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}