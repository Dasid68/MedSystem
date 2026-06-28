using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class Prescription
{
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public DateTime IssuedDate { get; set; } = DateTime.Now;
    public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(7);
    [StringLength(500)]
    public string? Instructions { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Medication { get; set; } = String.Empty;
    
}