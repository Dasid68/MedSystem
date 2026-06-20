namespace MedSystem.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    public string? ChornicConditions { get; set; }
    public string? Allergies { get; set; }
    public string? Notes { get; set; }
    
}