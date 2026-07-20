namespace MedSystem.Areas.Admin.Models;


public enum LogType
{
    Info = 1,
    Success = 2,
    Warning = 3,
    Danger = 4
}

public enum LogAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    Login = 4,
    Logout = 5,
}
public class SystemLog
{
    public int Id { get; set; }
    public string Message { get; set; } = String.Empty;
    public LogAction Action { get; set; }
    public LogType LogType { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}