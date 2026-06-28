using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public int CityId { get; set; }
    public City City { get; set; }
    
  
    public string Address { get; set; } = String.Empty;
    
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    
}