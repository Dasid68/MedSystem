using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;


public enum Gender
{
    Male,
    Female
}

public class Patient
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    [Required]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "ЕМБГ мора да содржи точно 13 цифри")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ЕМБГ мора да содржи само цифри")]
    public string EMBG { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    [RegularExpression(@"\+?\d{7,15}$", ErrorMessage = "Невалиден телефонски број")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Address { get; set; }
    
    public int? PrimaryDoctorId { get; set; }
    public Doctor? PrimaryDoctor { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Referral> Referrals { get; set; } = new List<Referral>();
    public MedicalRecord  MedicalRecord { get; set; }
}