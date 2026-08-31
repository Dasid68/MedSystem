using MedSystem.Enums;

namespace MedSystem.Areas.Doctor.Models;

public class AllAppointmentsViewModel
{
   
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string? Diagnosis { get; set; }
    
}