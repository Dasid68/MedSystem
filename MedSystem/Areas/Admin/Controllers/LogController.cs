using MedSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Admin.Controllers;

public class LogController(ApplicationDbContext context) : AdminBaseController
{
    // GET
    public IActionResult Index()
    {
        var logs = context.SystemLogs.ToList();
        return View(logs);
    }
}