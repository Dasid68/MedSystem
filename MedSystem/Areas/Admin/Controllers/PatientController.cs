using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Admin.Controllers;

public class PatientController : AdminBaseController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}