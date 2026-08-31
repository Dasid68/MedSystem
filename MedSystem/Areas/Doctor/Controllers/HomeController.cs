using MedSystem.Areas.Doctor.Models;
using MedSystem.Data;
using MedSystem.Enums;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Areas.Doctor.Controllers;
[Route("/doctor")]
public class HomeController
    (
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager 
    ) : DoctorBaseController
{
    // [Route("/doctor/home")]
    public IActionResult Index()
    {
        var doctorId = userManager.GetUserId(User);

        var doctor = context.Doctors
            .Include(d => d.ApplicationUser)
            .Include(d => d.Appointments)
            .ThenInclude(a => a.Patient)
            .ThenInclude(p => p.ApplicationUser)
            .Include(d => d.PrimaryPatients)
            .FirstOrDefault(d => d.ApplicationUserId ==  doctorId);

        var patients = context.Patients.Where(p => p.PrimaryDoctorId == doctor.Id)
            .Select(p => new
            {
                Id = p.Id,
                FullName = $"{p.ApplicationUser.FirstName} {p.ApplicationUser.LastName}"
            }).ToList();
        var p = new SelectList(patients, "Id", "FullName");
        ViewBag.Patients = p;

        var specialties = context.Specializations.ToList();
        var sSelectlist = new SelectList(specialties, "Id", "Name");
        ViewBag.Specializations = sSelectlist;

        var viewModel = new DoctorDashboardViewModel
        {
            DoctorName = doctor.ApplicationUser.FirstName + " " + doctor.ApplicationUser.LastName,
            TodayAppointments = doctor.Appointments
                .Where(a => a.AppointmentDate.Date == DateTime.Today && a.Status == Status.Confirmed)
                .ToList(),
            TotalPatientsCount = doctor.PrimaryPatients.Count,
            PendingAppointments = doctor.Appointments
                .Where(a => a.Status == Status.Pending)
                .ToList(),
            UpcomingAppointments = doctor.Appointments
                .Where(a => a.Status == Status.Confirmed && a.AppointmentDate >= DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .ToList()
        };
        
        
        return View(viewModel);
    }
    
    [Route("/doctor/patients")]
    public async Task<IActionResult> Patients()
    {
        var userId = userManager.GetUserId(User);

        var doctor = await context.Doctors
            .Include(d => d.PrimaryPatients)
            .ThenInclude(p => p.ApplicationUser)
            .Include(d => d.PrimaryPatients)
            .ThenInclude(p => p.Appointments)
            .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
        

        var patientsList = doctor.PrimaryPatients.Select(p => new DoctorPatientsViewModel
        {
            PatientId = p.Id,
            FullName = $"{p.ApplicationUser.FirstName} {p.ApplicationUser.LastName}",
            Email = p.ApplicationUser.Email ?? "/",
            Phone = p.ApplicationUser.PhoneNumber ?? "/",
            TotalAppointmentsCount = p.Appointments != null ? p.Appointments.Count : 0,
            LastAppointmentDate = p.Appointments != null && p.Appointments.Any(a => a.Status == Status.Completed)
                ? p.Appointments.Where(a => a.Status == Status.Completed).Max(a => a.AppointmentDate)
                : null
        }).ToList();

        return View(patientsList);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePrescription(int patientId, string medication, string instructions)
    {
        var userId = userManager.GetUserId(User);
        var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

        

        var prescription = new Prescription
        {
            PatientId = patientId,
            DoctorId = doctor.Id,
            Medication = medication,
            Instructions = instructions,
            IssuedDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddDays(30)
        };

        context.Prescriptions.Add(prescription);
        await context.SaveChangesAsync();

      
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    [Route("/Doctor/getdoctorsbyspecialty")]
    public async Task<IActionResult> GetDoctorsBySpecialty(string specialty)
    {
        if (string.IsNullOrEmpty(specialty)) return Json(new List<object>());

        var doctors = await context.Doctors
            .Where(d => d.Specialization.Id == Convert.ToInt32(specialty)) 
            .Select(d => new
            {
                id = d.Id,
                name = "Д-р " + d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName
            })
            .ToListAsync();

        return Json(doctors);
    }
    
    [HttpPost]
    [Route("/doctor/create-referral")]
    public async Task<IActionResult> CreateReferral(int patientId, int specialistId,int specializationId, string reason)
    {
        var userId = userManager.GetUserId(User);
        var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

      

        var referral = new Referral
        {
            PatientId = patientId,
            ReferringDoctorId = doctor.Id,
            ReferringSpecializationId =  specializationId,
            ReferredDoctorId = specialistId,
            Reason = reason,
            IssuedDate = DateTime.Now
        };

        context.Referrals.Add(referral);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}