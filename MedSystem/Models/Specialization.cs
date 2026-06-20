using System.ComponentModel.DataAnnotations;

namespace MedSystem.Models;

public class Specialization
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    
}