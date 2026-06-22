using System.ComponentModel.DataAnnotations;
using MedSystem.Enums;
using MedSystem.Models;

namespace MedSystem.Areas.Auth.Models;

public class RegisterPatientViewModel
{
    [Required(ErrorMessage = "Полето е задолжително")]
    public string FirstName { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public string LastName { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$", 
        ErrorMessage = "Лозинката мора да има најмалку 6 карактери и да содржи барем една голема буква, една мала буква, една бројка и еден специјален карактер (!@#$).")]
    public string Password { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    [EmailAddress(ErrorMessage = "Внесете валидна е-пошта.")]
    [Display(Name = "Е-пошта")] 
    public string Email { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public string PhoneNumber { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public int CityId { get; set; }
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public string Address { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "ЕМБГ мора да содржи точно 13 цифри")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ЕМБГ мора да содржи само цифри")]
    public string Embg { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Полето е задолжително")]
    public Gender Gender { get; set; }
    
    
    
}