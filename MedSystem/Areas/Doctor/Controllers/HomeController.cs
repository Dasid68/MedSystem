using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Doctor.Controllers;

public class HomeController : DoctorBaseController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}