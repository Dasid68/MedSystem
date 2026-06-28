namespace MedSystem.Models;

public class MedicalInstitution
{
    public int Id { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public string Address { get; set; } = String.Empty;
    
    public int CityId { get; set; }
    public City City { get; set; }
    
    
        
}