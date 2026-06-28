using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Admin.Controllers;

public class HomeController : AdminBaseController
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}