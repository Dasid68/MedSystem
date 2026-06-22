using System.ComponentModel.DataAnnotations;
using MedSystem.Enums;

namespace MedSystem.Models;


public class Patient
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    
   
    public string Embg { get; set; } = String.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
    public Gender Gender { get; set; }
    
    
    public int? PrimaryDoctorId { get; set; }
    public Doctor? PrimaryDoctor { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Referral> Referrals { get; set; } = new List<Referral>();
    public MedicalRecord  MedicalRecord { get; set; } = new MedicalRecord();
}