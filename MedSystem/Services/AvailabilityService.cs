using System.Net.Sockets;
using MedSystem.Data;
using MedSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Services;

public class AvailabilityService(ApplicationDbContext context)
{
    private static readonly TimeSpan WorkStart = new TimeSpan(8, 0, 0);
    private static readonly TimeSpan WorkEnd = new TimeSpan(16, 0, 0);

    public async Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return new List<TimeSpan>();
        }  
        
        var allSlots = new List<TimeSpan>();
        var current = WorkStart;

        while (current.Add(TimeSpan.FromMinutes(30)) <= WorkEnd)
        {
            allSlots.Add(current);
            current = current.Add(TimeSpan.FromMinutes(30));
        }

        var bookedTimes = await context.Appointments
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date && a.Status != Status.Cancelled)
            .Select(a => a.AppointmentDate.TimeOfDay).ToListAsync();
        
        return allSlots.Where(slot => !bookedTimes.Contains(slot)).ToList();


    }
}