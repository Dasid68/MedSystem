using MedSystem.Models;

namespace MedSystem.Areas.Admin.Models;

public class DashboardViewModel
{
    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TodayAppointments { get; set; }
    public int TotalUsers { get; set; }
    public List<Patient> RecentPatients { get; set; } = new List<Patient>();
    public List<SystemLog> RecentLogs { get; set; } = new List<SystemLog>();
}