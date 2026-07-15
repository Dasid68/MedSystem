using System.ComponentModel.DataAnnotations;

namespace MedSystem.Areas.Admin.Models;

public class EditPatientViewModel
{
    public int? Id { get; set; } 

    [Required(ErrorMessage = "Името е задолжително.")]
    [StringLength(50, ErrorMessage = "Името не може да биде подолго од 50 карактери.")]
    [Display(Name = "Име на пациент")]
    public string FirstName { get; set; } = String.Empty;

    [Required(ErrorMessage = "Презимето е задолжително.")]
    [StringLength(50, ErrorMessage = "Презимето не може да биде подолго од 50 карактери.")]
    [Display(Name = "Презиме на пациент")]
    public string LastName { get; set; } = String.Empty;

    [Required(ErrorMessage = "Е-маил адресата е задолжителна.")]
    [EmailAddress(ErrorMessage = "Внесете валиден формат за е-маил (на пр. ime@primer.com).")]
    [Display(Name = "Е-маил адреса")]
    public string Email { get; set; } =  String.Empty;
    
    [Display(Name="Матичен лекар")]
    public int? PrimaryDoctorId { get; set; }
    
    [Required(ErrorMessage = "Изборот на град е задолжителен.")]
    [Range(1, int.MaxValue, ErrorMessage = "Ве молиме изберете валиден град од листата.")]
    [Display(Name = "Град / Локација")]
    public int CityId { get; set; }
    
    [Required(ErrorMessage = "Телефонскиот број е задолжителен")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Невалиден телефонски број")]
    public string PhoneNumber { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Адресата е задолжителна")]
    public string Address { get; set; } = String.Empty;
}