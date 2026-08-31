namespace MedSystem.Areas.Doctor.Models;

public class DoctorPatientsViewModel
{
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
        
   
    public int TotalAppointmentsCount { get; set; }
    public DateTime? LastAppointmentDate { get; set; }
}