using MedSystem.Models;
using MedSystem.Areas.Auth.Models;
using MedSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedSystem.Controllers;

[Area("Auth")]
[AllowAnonymous]
[Route("auth")]
public class AccountController : Controller
{
   private readonly UserManager<ApplicationUser> _userManager;
   private readonly RoleManager<IdentityRole> _roleManager;
   private readonly SignInManager<ApplicationUser> _signInManager;
   private readonly ApplicationDbContext _context;

   public AccountController(
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager,
      SignInManager<ApplicationUser> signInManager,
      ApplicationDbContext dbContext
      
   )
   {
      _userManager = userManager;
      _roleManager = roleManager;
      _context = dbContext;
      _signInManager = signInManager;
   }
   [HttpGet("register")]
   public IActionResult RegisterPatient()
   { 
      ViewBag.Cities = new SelectList(_context.Cities.ToList(), "Id", "Name");
      return View();
   }
   [HttpGet("login")]
   public IActionResult Login()
   {
      return View();
   }
   [HttpPost("register")]
   public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
   {
     
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
         
         return RedirectToAction("Index", "Home");


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
         return RedirectToAction("Index", "Home", new {area = ""});
      }

      ViewBag.ErrorMessage = "Неуспешна најава. Проверете ја е-поштата и лозинката.";
      return View(model);

   }
}