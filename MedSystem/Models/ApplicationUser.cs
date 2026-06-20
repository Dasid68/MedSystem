using Microsoft.AspNetCore.Identity;

namespace MedSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // public Patient? Patient { get; set; }
    // public Doctor? Doctor { get; set; }
    
}