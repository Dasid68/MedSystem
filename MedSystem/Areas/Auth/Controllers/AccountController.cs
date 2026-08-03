using MedSystem.Areas.Admin.Models;
using MedSystem.Models;
using MedSystem.Areas.Auth.Models;
using MedSystem.Data;
using MedSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedSystem.Areas.Auth.Controllers;

[Area("Auth")]
[AllowAnonymous]
[Route("auth")]
public class AccountController : Controller
{
   private readonly UserManager<ApplicationUser> _userManager;
   private readonly SignInManager<ApplicationUser> _signInManager;
   private readonly ApplicationDbContext _context;
   private readonly ISystemLogService _logService;

   public AccountController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      ApplicationDbContext dbContext,
      ISystemLogService logService
   )
   {
      _userManager = userManager;
      _context = dbContext;
      _signInManager = signInManager;
      _logService = logService;
   }
   [HttpGet("register")]
   public IActionResult RegisterPatient()
   { 
      if (_signInManager.IsSignedIn(User))
      {
         return RedirectToAction("Index", "Home", new {area=""});
      }
      
      ViewBag.Cities = new SelectList(_context.Cities.ToList(), "Id", "Name");
      return View();
   }
   [HttpGet("login")]
   public IActionResult Login()
   {
      if (_signInManager.IsSignedIn(User))
      {
         return RedirectToAction("Index", "Home", new {area=""});
      }
      return View();
   }
   [HttpPost("register")]
   public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
   {
      
      if (_signInManager.IsSignedIn(User))
      {
         return RedirectToAction("Index", "Home", new {area=""});
      }
     
      if (!ModelState.IsValid)
      {
         var cities = _context.Cities.ToList();
      
         ViewBag.Cities = new SelectList(cities, "Id", "Name");
         return View(model);
      }

      var user = new ApplicationUser
      {
         FirstName = model.FirstName,
         LastName = model.LastName,
         UserName = model.Email,
         Email = model.Email,
         PhoneNumber = model.PhoneNumber,
         Address =  model.Address,
         CityId = model.CityId,
         EmailConfirmed = true
      };
      
      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
         await _userManager.AddToRoleAsync(user, "Patient");
         
         var patient = new Patient
         {
            ApplicationUserId = user.Id,
            DateOfBirth =  model.DateOfBirth,
            Gender = model.Gender,
            Embg =  model.Embg,
            
            
         };
         
         _context.Patients.Add(patient);
         await _context.SaveChangesAsync();
         
         await _signInManager.SignInAsync(user, false);
         
         await _logService.LogAsync(
            message: $"Успешно регистриран нов пациент: {user.FirstName} {user.LastName}",
            action: LogAction.Create,
            logType: LogType.Success,
            performedBy: User.Identity?.Name
         );
         
         return RedirectToAction("Index", "Home", new {area = ""});


      } 
      
      ViewBag.Cities = new SelectList(_context.Cities.ToList(), "Id", "Name");
      return View(model);
      
      
   }

   [HttpPost("login")]
   public async Task<IActionResult> Login(LoginViewModel model)
   {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

      if (result.Succeeded)
      {
         var user = await _userManager.FindByNameAsync(model.Email);
         
         if (user != null)
         {
            var roles = await _userManager.GetRolesAsync(user);
            
            if (roles.Contains("Patient"))
            {
               return RedirectToAction("Index", "Home", new {area = ""});
            }

            if (roles.Contains("Doctor"))
            {
               return RedirectToAction("Index", "Home", new {area = "Doctor"});
            }

            if (roles.Contains("Admin"))
            {
               return RedirectToAction("Index", "Home", new {area = "Admin"});
            }
         }

         
      } 


      ViewBag.ErrorMessage = "Неуспешна најава. Проверете ја е-поштата и лозинката.";
      return View(model);

   }

   [HttpPost("logout")]
   public async Task<IActionResult> Logout()
   {
      await _signInManager.SignOutAsync();

      return RedirectToAction("Index", "Home", new { area = "" });
   }
}