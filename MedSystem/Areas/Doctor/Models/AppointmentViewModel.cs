using MedSystem.Models;

namespace MedSystem.Areas.Doctor.Models;


public class PrescriptionItemDto
{
    public string Medication { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
}

public class AppointmentViewModel
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;

    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string PatientEmail { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;
    public string? EmergencyContact { get; set; }
    public string? EMBG { get; set; } 

    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? PrescriptionsJson{ get; set; } = string.Empty;
    public string? Notes { get; set; }

    public List<Appointment> PastAppointments { get; set; } = new();
}