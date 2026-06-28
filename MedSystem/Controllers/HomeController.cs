using System.Diagnostics;
using System.Security.Claims;
using MedSystem.Data;
using Microsoft.AspNetCore.Mvc;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Controllers;

public class HomeController(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context    
    ) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (signInManager.IsSignedIn(User))
        {
            var _user = await userManager.GetUserAsync(User);
         
            if (_user != null)
            {
                var roles = await userManager.GetRolesAsync(_user);
                
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
        
        
        string userEmail = User.FindFirstValue(ClaimTypes.Email)!;

        var user = context.Users.Include(u => u.Patient).ThenInclude(p => p.Referrals).FirstOrDefault(u => u.Email == userEmail);
        
        
        return View(user);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}