using MedSystem.Areas.Admin.Models;
using MedSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Areas.Admin.Controllers;



// TODO: Dodadi prebaruvanje na pacienti i doktori
public class PatientController(
    ApplicationDbContext context
    )
    : AdminBaseController
{
 
    public IActionResult Index()
    {
        var patients = context.Patients
            .Include(p => p.ApplicationUser)
            .Include(p => p.PrimaryDoctor)
            .ThenInclude(pd => pd!.ApplicationUser)
            .ToList();
        return View(patients);
    }

    public IActionResult Edit(int id)
    {
        var patient = context.Patients.Include(p => p.ApplicationUser).FirstOrDefault(p => p.Id == id);
        var editModel = new EditPatientViewModel
        {
            FirstName = patient!.ApplicationUser.FirstName,
            LastName = patient.ApplicationUser.LastName,
            Email = patient.ApplicationUser.Email!,
            Address = patient.ApplicationUser.Address,
            PhoneNumber = patient.ApplicationUser.PhoneNumber!,
            PrimaryDoctorId = patient.PrimaryDoctorId,
            CityId = patient.ApplicationUser.CityId,
        };
        
        ViewBag.Doctors = new SelectList(context.Doctors.Select(
            d => new
            {
                Id = d.Id,
                FullName = d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName
            }), "Id", "FullName");
        ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
        
        return View(editModel);
    }

    public async Task<IActionResult> ConfirmEdit(EditPatientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = new SelectList(context.Doctors.Select(
                    d => new
                    {
                        Id = d.Id,
                        FullName = d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName
                    }), "Id", "FullName");
                ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
                return View("Edit",model);
            }
        
        var patient = await context.Patients.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.Id == model.Id);
        
        patient!.PrimaryDoctorId = model.PrimaryDoctorId;
        patient.ApplicationUser.FirstName = model.FirstName;
        patient.ApplicationUser.LastName = model.LastName;
        patient.ApplicationUser.PhoneNumber = model.PhoneNumber;
        patient.ApplicationUser.CityId = model.CityId;
        patient.ApplicationUser.Address = model.Address;
        patient.ApplicationUser.Email = model.Email;
        
        
        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Patient", new { area = "Admin" });
    }
    public IActionResult AssignDoctor(int id)
    {
        var patient = context.Patients.Find(id);
        return View(patient);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var patient = await context.Patients.FindAsync(id);
        context.Patients.Remove(patient!);
        await context.SaveChangesAsync();
        return RedirectToAction("Index", "Patient", new { area = "Admin" });
    }
    
    
    public async Task<IActionResult> GetDoctorByName(string searchQuery)
    {
        var doctorsQuery =  context.Doctors.Include(d => d.ApplicationUser).AsQueryable();

        if (string.IsNullOrEmpty(searchQuery) || searchQuery.Length < 2)
        {
            return Content("");
        }

        searchQuery = searchQuery.Trim().ToLower();
            doctorsQuery = doctorsQuery.Where(d => (d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName).ToLower().Contains(searchQuery) ||
                                                   d.ApplicationUser.FirstName.Contains(searchQuery) || d.ApplicationUser.LastName.Contains(searchQuery));
            
            
        
        
        var doctors = await doctorsQuery.ToListAsync();
        return PartialView("Partials/_SearchResult",doctors);
    }

    [HttpPost]
    public async Task<IActionResult> AssignDoctor(int patientId, int doctorId)
    {
        var patient = await context.Patients.FindAsync(patientId);
        patient!.PrimaryDoctorId = doctorId;
        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Patient", new { area = "Admin" });

    }

    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var patient = await context.Patients
            .Include(p => p.PrimaryDoctor)
            .ThenInclude(pd => pd!.ApplicationUser)
            .Include(p => p.ApplicationUser)
            .ThenInclude(ap => ap.City)
            .FirstOrDefaultAsync(p => p.Id == id);
        return PartialView("Partials/_PatientDetailsContent",patient);        
    }
}