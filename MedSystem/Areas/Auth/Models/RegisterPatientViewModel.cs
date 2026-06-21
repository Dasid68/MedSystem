using System.ComponentModel.DataAnnotations;
using MedSystem.Models;

namespace MedSystem.Areas.Auth.Models;

public class RegisterPatientViewModel
{
    [Required]
    public string FirstName { get; set; } = String.Empty;
    
    [Required]
    public string LastName { get; set; } = String.Empty;
    
    [Required]
    public string Password { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето за е-пошта е задолжително.")]
    [EmailAddress(ErrorMessage = "Внесете валидна е-пошта.")]
    [Display(Name = "Е-пошта")] 
    public string Email { get; set; } = String.Empty;
    
    [Required]
    public string PhoneNumber { get; set; } = String.Empty;
    
    [Required]
    public int CityId { get; set; }
    
    [Required]
    public string Address { get; set; } = String.Empty;
    
    [Required]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "ЕМБГ мора да содржи точно 13 цифри")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ЕМБГ мора да содржи само цифри")]
    public string Embg { get; set; } = String.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    
    
}