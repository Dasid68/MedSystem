using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class Doctor
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    
   
    public int? SpecializationId { get; set; }
    public Specialization  Specialization { get; set; }
    
   

    public ICollection<Referral> IssuedReferrals { get; set; } = new List<Referral>();
    public ICollection<Referral> ReceivedReferrals { get; set; } = new List<Referral>();
    public ICollection<Patient> PrimaryPatients { get; set; } = new List<Patient>(); 
}