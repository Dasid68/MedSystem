using MedSystem.Areas.Admin.Models;
using MedSystem.Data;
using MedSystem.Models;
using MedSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Areas.Admin.Controllers;

public class SpecializationController(ApplicationDbContext context, ISystemLogService logService) : AdminBaseController
{
    
    public IActionResult Index()
    {
        var specializations = context.Specializations.OrderByDescending(s => s.Id).ToList();
        return View(specializations);
    }

    public IActionResult Details(int id)
    {
        var specialization = context.Specializations.Find(id);
        var doctors = context.Doctors.Include(d => d.ApplicationUser).Where(d => d.SpecializationId == id).ToList();
        ViewBag.Name = specialization!.Name;
        ViewBag.Id = specialization.Id;
        return View(doctors);
    }

    public IActionResult Edit(int id)
    {
        var specialization = context.Specializations.Find(id);
        return View(specialization);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Specialization model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        Console.WriteLine(model);
        var spec = await context.Specializations.FindAsync(model.Id);
        var oldName = spec.Name;
        var newName = model.Name;
        spec!.Name = model.Name;
        await context.SaveChangesAsync();
        
        await logService.LogAsync(
            message: $"Специјализацијата '{oldName}' е преименувана во '{newName}'",
            action: LogAction.Update,
            logType: LogType.Info,
            performedBy: User.Identity?.Name
        );
        
        return RedirectToAction("Index", "Specialization", new { area = "Admin" });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Specialization specialization)
    {
        context.Specializations.Add(specialization);
        await context.SaveChangesAsync();
        
        await logService.LogAsync(
            message: $"Додадена е нова специјализација: '{specialization.Name}'",
            action: LogAction.Create,
            logType: LogType.Success,
            performedBy: User.Identity?.Name
        );
        
        return RedirectToAction("Index", "Specialization", new { area = "Admin" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var spec = await context.Specializations.FindAsync(id);
        var doctors = await context.Doctors.Where(d => d.SpecializationId == id).ToListAsync();

        foreach (var doctor in doctors)
        {
            doctor.SpecializationId = null;
        }
        
        context.Specializations.Remove(spec!);
        await context.SaveChangesAsync();
        
        await logService.LogAsync(
            message: $"Избришана е специјализацијата: '{spec.Name}'",
            action: LogAction.Delete,
            logType: LogType.Danger,
            performedBy: User.Identity?.Name
        );
        
        return RedirectToAction("Index", "Specialization", new { area = "Admin" });
    }

    public async Task<IActionResult> RemoveDoctorFromSpecialization(int id, int specializationId)
    {
        var doctor = await context.Doctors.FindAsync(id);
        doctor!.SpecializationId = null;
        
        await context.SaveChangesAsync();
        return RedirectToAction("Details", "Specialization", new { area = "Admin" , id = specializationId});
    }
}