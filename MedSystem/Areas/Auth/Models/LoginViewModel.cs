using System.ComponentModel.DataAnnotations;

namespace MedSystem.Areas.Auth.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Полето за е-пошта е задолжително.")]
    [EmailAddress(ErrorMessage = "Внесете валидна е-пошта.")]
    [Display(Name = "Е-пошта")]
    public string Email { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Лозинката е задолжителна")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
    
}