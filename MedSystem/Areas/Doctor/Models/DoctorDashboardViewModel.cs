using MedSystem.Models;

namespace MedSystem.Areas.Doctor.Models;

public class DoctorDashboardViewModel
{
    public string DoctorName { get; set; } = string.Empty;
    public int TodayAppointmentsCount { get; set; }
    public int TotalPatientsCount { get; set; }

    public List<Appointment> PendingAppointments { get; set; } = new();
    public List<Appointment> UpcomingAppointments { get; set; } = new();
    public List<Appointment> TodayAppointments { get; set; } = new();
}