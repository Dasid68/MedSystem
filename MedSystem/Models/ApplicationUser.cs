using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public int CityId { get; set; }
    public City City { get; set; }
    
  
    public string Address { get; set; }
    
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    
}