using System.Security.Claims;
using MedSystem.Data;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Controllers;

public class AppointmentController
    (
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager
    )
    : Controller
{
    [Route("/appointments")]
    public IActionResult Index()
    {
        
        var userId = userManager.GetUserId(User);
        var appointments = context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Where(a => a.Patient.ApplicationUserId == userId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToList();

        ViewBag.UserId = userId;

        
        string userEmail = User.FindFirstValue(ClaimTypes.Email)!;

        var user = context.Users
            .Include(u => u.Patient)
            .ThenInclude(p => p.Appointments.OrderByDescending(ap => ap.AppointmentDate))
            .Include(u => u.Patient)
            .ThenInclude(p => p.PrimaryDoctor)
            .ThenInclude(d => d.ApplicationUser)
            .FirstOrDefault(u => u.Email == userEmail);
        
        ViewBag.User = user;
        return View(appointments);
        
       
    }
}