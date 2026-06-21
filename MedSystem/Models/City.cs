using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class City
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}