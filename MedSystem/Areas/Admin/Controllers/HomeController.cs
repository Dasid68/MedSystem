using MedSystem.Areas.Admin.Models;
using MedSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Areas.Admin.Controllers;

public class HomeController(ApplicationDbContext context) : AdminBaseController
{
    [HttpGet("")]
    public IActionResult Index()
    {
        var totalPatients = context.Patients.Count();
        var totalDoctors = context.Doctors.Count();
        var todayAppointments = context.Appointments.Count(a => a.AppointmentDate.Equals(DateTime.Today));
        var totalUsers = context.Users.Count();

        var recentPatients = context.Patients
            .Include(p => p.ApplicationUser)
            .ThenInclude(u => u.City)
            .Include(p => p.PrimaryDoctor)
            .ThenInclude(d => d.ApplicationUser)
            .OrderByDescending(p => p.Id)
            .Take(5)
            .ToList();
        
        var recentLogs = context.SystemLogs.Take(5).ToList();
        
        var model = new DashboardViewModel
        {
            TotalPatients = totalPatients,
            TotalDoctors = totalDoctors,
            TotalUsers = totalUsers,
            TodayAppointments = todayAppointments,
            RecentPatients = recentPatients,
            RecentLogs = recentLogs,
        };
        
        return View(model);
    }
}