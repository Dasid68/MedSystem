using MedSystem.Areas.Admin.Models;
using MedSystem.Data;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MedSystem.Areas.Admin.Controllers;

using DoctorModel = MedSystem.Models.Doctor;


public class DoctorController(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context
    ) : AdminBaseController
{
    public async Task<IActionResult> Index()
    {
        var doctors = await context.Doctors.Include(d => d.ApplicationUser).Include(d => d.Specialization).ToListAsync();
        
        return View(doctors);
    }

    public IActionResult Create()
    {
        ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
        ViewBag.Specializations = new SelectList(context.Specializations, "Id", "Name");
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(DoctorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
            ViewBag.Specializations = new SelectList(context.Specializations, "Id", "Name");

            return View(model);
        }

        try
        {

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                CityId = model.CityId,
                Address = model.Address,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Doctor");
                
                var doctor = new DoctorModel
                {
                    ApplicationUserId = user.Id,
                    SpecializationId = model.SpecializationId
                };

                context.Doctors.Add(doctor);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "Doctor", new { area = "Admin" });


            }
            Console.Error.WriteLine(result.Errors);
            ViewBag.Error = "Настана грешка, обидете се повторно.";
        }
        catch (Exception)
        { 
            ViewBag.Error = "Настана грешка, обидете се повторно.";
        }
        
        
        ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
        ViewBag.Specializations = new SelectList(context.Specializations, "Id", "Name");
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var doctor = await context.Doctors.Include(d => d.ApplicationUser).Include(d => d.Specialization).SingleOrDefaultAsync(m => m.Id == id);

        var editModel = new EditDoctorViewModel
        {
            Address = doctor!.ApplicationUser.Address,
            Email = doctor.ApplicationUser.Email!,
            FirstName = doctor.ApplicationUser.FirstName,
            LastName = doctor.ApplicationUser.LastName,
            PhoneNumber = doctor.ApplicationUser.PhoneNumber!,
            CityId = doctor.ApplicationUser.CityId,
            SpecializationId = doctor.SpecializationId,
        };
        ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
        ViewBag.Specializations = new SelectList(context.Specializations, "Id", "Name");
        return View(editModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditDoctorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Cities = new SelectList(context.Cities, "Id", "Name");
            ViewBag.Specializations = new SelectList(context.Specializations, "Id", "Name");
            return View(model);
        }
        
        var doctor = await context.Doctors.Include(d => d.ApplicationUser).Include(d => d.Specialization).SingleOrDefaultAsync(m => m.Id == id);
        doctor!.ApplicationUser.FirstName = model.FirstName;
        doctor.ApplicationUser.LastName = model.LastName;
        doctor.ApplicationUser.Address = model.Address;
        doctor.ApplicationUser.Email = model.Email;
        doctor.ApplicationUser.PhoneNumber = model.PhoneNumber;
        doctor.ApplicationUser.CityId = model.CityId;
        doctor.SpecializationId = model.SpecializationId;

        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Doctor", new { area = "Admin" });
    }

    public async Task<IActionResult> AddPatient(int? id)
    {
        ViewBag.DoctorId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddPatientToDoctor(int doctorId, int patientId)
    {
        var doctor = await context.Doctors.FindAsync(doctorId);
        var patient = await context.Patients.FindAsync(patientId);
        
        doctor!.PrimaryPatients.Add(patient!);
        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Doctor", new { area = "Admin" });
    }

    public async Task<IActionResult> GetPatientByEMBG(string embg)
    {
        if (embg.IsNullOrEmpty())
        {
            return BadRequest("ЕМБГ е задолжително");
        }
        
        var patient = await context.Patients.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.Embg == embg);
        if (patient == null)
            return NotFound();
        
        
        
        return PartialView("Partials/_PatientInfo", patient);
    }

    public async Task<IActionResult> Details(int id)
    {
        var doctor = await context.Doctors.Include(d => d.PrimaryPatients)
            .ThenInclude(p => p.ApplicationUser)
            .Include(d => d.ApplicationUser)
            .ThenInclude(a => a.City)
            .Include(d => d.Specialization)
            .FirstOrDefaultAsync(d => d.Id == id);
        return View(doctor);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await context.Doctors.Include(d => d.ApplicationUser).FirstOrDefaultAsync(d => d.Id == id);

        context.Doctors.Remove(doctor!);
        context.Users.Remove(doctor!.ApplicationUser);
        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Doctor", new { area = "Admin" });
    }
    
}