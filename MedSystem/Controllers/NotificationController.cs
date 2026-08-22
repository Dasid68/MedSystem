using MedSystem.Models;
using MedSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Controllers;

[Authorize]
public class NotificationController(
   INotificationService notificationService,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        await notificationService.MarkAllAsReadAsync(userId);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var success = await notificationService.DeleteAsync(id, userId);
        if (success) return Ok();

        return NotFound();
    }
}