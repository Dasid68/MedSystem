using MedSystem.Areas.Admin.Models;
using MedSystem.Data;

namespace MedSystem.Services
{

    public interface ISystemLogService
    {
        Task LogAsync(string message, LogAction action, LogType logType, string? performedBy = null);
    }

    public class SystemLogService(ApplicationDbContext context) : ISystemLogService
    {
        public async Task LogAsync(string message, LogAction action, LogType logType, string? performedBy = null)
        {
            var log = new SystemLog
            {
                Message = message,
                Action = action,
                LogType = logType,
                PerformedBy = performedBy,
                Timestamp = DateTime.Now
            };

            context.SystemLogs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}