using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models
{
    public class PatientProfileViewModel
    {
        [Required(ErrorMessage = "Името е задолжително.")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Презимето е задолжително.")]
        [Display(Name = "Презиме")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Телефонски број")]
        [Required(ErrorMessage = "Невалиден телефонски број.")]
        
        public string PhoneNumber { get; set; }

        [Display(Name = "Адреса на живеење")]
        public string Address { get; set; }

        [Display(Name = "Датум на раѓање")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Е-маил адреса")]
        public string Email { get; set; } = string.Empty;
    }
}