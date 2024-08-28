using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RestApiWithServiceWorker.Controller;

[Route("/Process")]
public class ProcessController : Microsoft.AspNetCore.Mvc.Controller
{
    // private IDataStore DataStore { get; set; }
    //
    // public ProcessController(IDataStore dataStore)
    // {
    //     DataStore = dataStore;
    // }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // var sc = DataStore.GetSc();
        //
        // ViewBag.QueryString = $"~/{sc.messageResponse.QueryString}";
        return View();
    }
}