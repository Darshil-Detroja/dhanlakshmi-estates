using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Admin controller for managing users
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminUserController> _logger;

        public AdminUserController(ApplicationDbContext context, ILogger<AdminUserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: AdminUser/Index
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();

            return View(users);
        }

        // GET: AdminUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Inquiries)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AdminUser/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {user.Email} status changed to {(user.IsActive ? "Active" : "Inactive")}");
            TempData["Success"] = $"User {user.Email} has been {(user.IsActive ? "activated" : "deactivated")}.";

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminUser/ApproveUpdate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUpdate(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !user.HasPendingUpdates)
            {
                return NotFound();
            }

            try
            {
                // Deserialize pending updates
                var pendingData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(user.PendingUpdates!);
                
                if (pendingData != null)
                {
                    // Apply updates
                    if (pendingData.ContainsKey("FirstName")) user.FirstName = pendingData["FirstName"].GetString() ?? user.FirstName;
                    if (pendingData.ContainsKey("LastName")) user.LastName = pendingData["LastName"].GetString() ?? user.LastName;
                    if (pendingData.ContainsKey("PhoneNumber")) user.PhoneNumber = pendingData["PhoneNumber"].GetString();
                    if (pendingData.ContainsKey("Address")) user.Address = pendingData["Address"].GetString();
                    if (pendingData.ContainsKey("City")) user.City = pendingData["City"].GetString();
                    if (pendingData.ContainsKey("State")) user.State = pendingData["State"].GetString();
                    if (pendingData.ContainsKey("ZipCode")) user.ZipCode = pendingData["ZipCode"].GetString();
                }

                user.HasPendingUpdates = false;
                user.PendingUpdates = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Admin approved profile update for user {user.Email}");
                TempData["Success"] = $"Profile update for {user.Email} has been approved.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving profile update for user {user.Email}");
                TempData["Error"] = "An error occurred while approving the update.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: AdminUser/RejectUpdate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUpdate(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !user.HasPendingUpdates)
            {
                return NotFound();
            }

            user.HasPendingUpdates = false;
            user.PendingUpdates = null;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin rejected profile update for user {user.Email}");
            TempData["Success"] = $"Profile update for {user.Email} has been rejected.";

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: AdminUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AdminUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User deleted: {user.Email} (ID: {user.UserId})");
                TempData["Success"] = "User deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
