using MedSystem.Data;
using MedSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.ViewComponents;

public class NotificationBellViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ViewComponent
{
   public async Task<IViewComponentResult> InvokeAsync()
   {
      var userId = userManager.GetUserId(UserClaimsPrincipal);

      var notifications = await context.Notifications
         .Where(n => n.UserId == userId)
         .OrderByDescending(n => n.CreatedAt)
         .Take(5)
         .ToListAsync();
      return View(notifications);
   }
}