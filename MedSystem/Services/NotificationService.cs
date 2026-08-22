using MedSystem.Data;
using MedSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Services;

public interface INotificationService
{
    Task CreateAsync(string userId, string message, NotificationType type, string? link = null);
    Task<List<Notification>> GetUserNotificationsAsync(string userId);
    Task MarkAllAsReadAsync(string userId);
    Task<bool> DeleteAsync(int id, string userId);
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

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var unread = await context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var n in unread)
        {
            n.IsRead = true;
        }

        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (notification != null)
        {
            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}