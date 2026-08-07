using System.ComponentModel.DataAnnotations;
using MedSystem.Enums;

namespace MedSystem.Models;


public class Appointment
{
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    [StringLength(300)]
    public string Reason { get; set; }

    public Status Status { get; set; } = Status.Pending;
    
    public string? Notes { get; set; }
}