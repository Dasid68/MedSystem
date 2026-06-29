using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Admin.Controllers;

public class SpecializationController : AdminBaseController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}