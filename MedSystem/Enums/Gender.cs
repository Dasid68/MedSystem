using System.ComponentModel.DataAnnotations;

namespace MedSystem.Enums;

public enum Gender
{
    [Display(Name = "Машки")]
    Male,
    [Display(Name="Женски")]
    Female
}