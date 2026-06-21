using MedSystem.Models;
using MedSystem.Areas.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Controllers;

[Area("Auth")]
[AllowAnonymous]
[Route("auth")]
public class AccountController : Controller
{
   private readonly UserManager<ApplicationUser> _userManager;

   public AccountController(UserManager<ApplicationUser> userManager)
   {
      _userManager = userManager;
   }
   [HttpGet("register")]
   public IActionResult RegisterPatient()
   {
      return View();
   }
   [HttpGet("login")]
   public IActionResult Login()
   {
      return View();
   }
   
   [HttpPost]
   public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
   {
      Console.WriteLine(model.FirstName + " " + model.LastName);
      if (!ModelState.IsValid)
      {
         return View(model);
      }
      return RedirectToAction("Index", "Home");
   }

   public async Task<IActionResult> LoginPatient()
   {
      return RedirectToAction("Index", "Home");
   }
}