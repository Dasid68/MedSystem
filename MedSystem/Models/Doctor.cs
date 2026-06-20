using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class Doctor
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    [Required]
    public int SpecializationId { get; set; }
    public Specialization  Specialization { get; set; }
    
    [Required]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Невалиден телефонски број")]
    public string PhoneNumber { get; set; }

    public ICollection<Referral> IssuedReferrals { get; set; } = new List<Referral>();
    public ICollection<Referral> ReceivedReferrals { get; set; } = new List<Referral>();
    public ICollection<Patient> PrimaryPatients { get; set; } = new List<Patient>(); 
}