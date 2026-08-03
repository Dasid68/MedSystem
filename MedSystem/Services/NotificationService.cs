using MedSystem.Data;
using MedSystem.Models;

namespace MedSystem.Services;

public interface INotificationService
{
    Task CreateAsync(string userId, string message, NotificationType type, string? link = null);
}

public class NotificationService(ApplicationDbContext context) : INotificationService
{
    public async Task CreateAsync(string userId, string message, NotificationType type, string? link = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            Type = type,
            Link = link
        };
        context.Notifications.Add(notification);
        await context.SaveChangesAsync();
    }
}