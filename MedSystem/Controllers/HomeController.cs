using System.Diagnostics;
using System.Security.Claims;
using MedSystem.Data;
using MedSystem.Enums;
using Microsoft.AspNetCore.Mvc;
using MedSystem.Models;
using MedSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Controllers;

public class HomeController(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context,
    INotificationService notificationService
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

        var user = context.Users
            .Include(u => u.Patient)
                .ThenInclude(p => p.Appointments.OrderByDescending(ap => ap.AppointmentDate))
            .Include(u => u.Patient)
                .ThenInclude(p => p.PrimaryDoctor)
                .ThenInclude(d => d.ApplicationUser)
            .Include(u => u.Patient)
                .ThenInclude(p => p.Referrals)
                .ThenInclude(r => r.ReferringSpecialization)
            .Include(u => u.Patient)
                .ThenInclude(p => p.Referrals)
                .ThenInclude(r => r.ReferringDoctor)
                .ThenInclude(d => d.ApplicationUser)
            .Include(u => u.Patient)
                .ThenInclude(p => p.Referrals)
                .ThenInclude(r => r.ReferredDoctor)
                .ThenInclude(d => d.ApplicationUser)
            .Include(u => u.Patient)
            .ThenInclude(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .FirstOrDefault(u => u.Email == userEmail);
        
        var userId = userManager.GetUserId(User);
        var now = DateTime.Now;
        var threeDaysFromNow = now.AddDays(3);
        
        var upcomingAppointment = await context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .Where(a => a.Patient.ApplicationUserId == userId &&
                        a.AppointmentDate >= now && 
                        a.AppointmentDate <= threeDaysFromNow &&
                        a.Status == Status.Confirmed) 
            .OrderBy(a => a.AppointmentDate)
            .FirstOrDefaultAsync();

        
        ViewBag.UpcomingAppointment = upcomingAppointment;
        
        var activeReceipts = context.Users.Include(u => u.Patient).ThenInclude(p => p.Prescriptions)
            
            .FirstOrDefault(u => u.Email == userEmail);

        var t = activeReceipts.Patient.Prescriptions.Where(p => (p.ExpirationDate - p.IssuedDate).TotalDays < 7);
        ViewBag.ActiveReceipts = t;
        
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
    
    [HttpPost]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var userId = userManager.GetUserId(User);
        if (userId != null)
        {
            await notificationService.MarkAllAsReadAsync(userId);
            return Ok();
        }
        return BadRequest();
    }
    [Route("/referrals")]
    public async Task<IActionResult> Referrals()
    {
        var user = await userManager.GetUserAsync(User);
        var patient = context.Patients.FirstOrDefault(p => p.ApplicationUserId == user.Id);

        var referrals = context.Referrals.Where(r => r.PatientId == patient.Id)
            .Include(r => r.ReferringDoctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(r => r.ReferredDoctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(r => r.ReferringSpecialization)
            .ToList();
            
        
        
        
        return View(referrals);
    }
    [Route("/prescriptions")]
    public async Task<IActionResult> Prescriptions()
    {
        var user = await userManager.GetUserAsync(User);
        var patient = context.Patients.FirstOrDefault(p => p.ApplicationUserId == user.Id);

        var prescriptions = context.Prescriptions.Include(p => p.Doctor).ThenInclude(d => d.ApplicationUser).Where(p => p.PatientId == patient.Id);

        return View(prescriptions);
    }
    
    
}