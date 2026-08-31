using MedSystem.Areas.Admin.Controllers;
using MedSystem.Areas.Doctor.Models;
using MedSystem.Data;
using MedSystem.Enums;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace MedSystem.Areas.Doctor.Controllers;
[Route("/doctor/appointment")]
public class AppointmentController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager
    ) : DoctorBaseController
{
    public IActionResult Index(int id)
    {
        var app = context.Appointments.Include(a => a.Patient).ThenInclude(p => p.ApplicationUser).First(a => a.Id == id);
        var pastAppointments = context.Appointments
            .Where(a => a.PatientId == app.PatientId 
                        && a.Id != id 
                        && a.Status == Status.Completed)
            .OrderByDescending(a => a.AppointmentDate)
            .ToList();
        
        var model = new AppointmentViewModel
        {
            AppointmentId = app.Id,
            AppointmentDate = app.AppointmentDate,
            Reason = app.Reason,
            Notes = app.Notes,
            
            PatientId = app.PatientId,
            PatientFullName = $"{app.Patient.ApplicationUser.FirstName} {app.Patient.ApplicationUser.LastName}",
            PatientEmail = app.Patient.ApplicationUser.Email ?? "/",
            PatientPhone = app.Patient.ApplicationUser.PhoneNumber ?? "/",

            PastAppointments = pastAppointments
        };
        
        
        return View(model);
    }

 
    [HttpPost]
   
    public async Task<IActionResult> Complete(AppointmentViewModel model)
    {
        var appointment = await context.Appointments.FindAsync(model.AppointmentId);
        if (appointment == null) return NotFound();

 
        appointment.Symptoms = model.Symptoms;
        appointment.Diagnosis = model.Diagnosis;
        appointment.Notes = model.Notes;
        appointment.Status = Status.Completed;

      
        var userId = userManager.GetUserId(User);
        var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

     
        if (!string.IsNullOrEmpty(model.PrescriptionsJson) && doctor != null)
        {
            var items = JsonSerializer.Deserialize<List<PrescriptionItemDto>>(
                model.PrescriptionsJson, 
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrWhiteSpace(item.Medication)) continue;

                    var prescription = new Prescription
                    {
                        PatientId = model.PatientId,
                        DoctorId = doctor.Id,
                        Medication = item.Medication,
                        Instructions = item.Instructions,
                        IssuedDate = DateTime.Now,
                        ExpirationDate = DateTime.Now.AddDays(7)
                    };

                    context.Prescriptions.Add(prescription);
                }
            }
        }

        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Home", new { area = "Doctor" });
    }
    
    [Route("/all-appointments")]
    public async Task<IActionResult> AllAppointments()
    {
        var userId = userManager.GetUserId(User);
    
        var doctor = await context.Doctors
            .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

        if (doctor == null)
        {
            return NotFound("Докторот не е пронајден.");
        }

        var appointments = await context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p.ApplicationUser)
            .Where(a => a.DoctorId == doctor.Id)
            .OrderByDescending(a => a.AppointmentDate) 
            .Select(a => new AllAppointmentsViewModel()
            {
                Id = a.Id,
                PatientName = $"{a.Patient.ApplicationUser.FirstName} {a.Patient.ApplicationUser.LastName}",
                PatientPhone = a.Patient.ApplicationUser.PhoneNumber ?? "/",
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status,
                Diagnosis = a.Diagnosis
            })
            .ToListAsync();

        return View(appointments);
    }
}