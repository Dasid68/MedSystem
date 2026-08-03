namespace MedSystem.Models;

public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set;}
    
    public string Message { get; set; } = String.Empty;
    public NotificationType Type { get; set; }

    public bool IsRead { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public string? Link { get; set; }


}

public enum NotificationType
{
    LabResult,
    AppointmentConfirmed,
    AppointmentDeclined,
    NewPrescription,
    NewReferral
}