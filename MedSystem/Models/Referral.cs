using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class Referral
{
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    [Required]
    
    public int ReferringDoctorId { get; set; }
    public Doctor ReferringDoctor { get; set; }
    
    [Required]
    public int ReferredDoctorId { get; set; }
    public Doctor ReferredDoctor { get; set; }
    
    public DateTime IssuedDate { get; set; } =  DateTime.Now;
    
    [Required]
    [StringLength(300)]
    public string Reason { get; set; }
     
    
    
    
}