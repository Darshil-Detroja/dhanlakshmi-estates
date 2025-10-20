using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Admin controller for managing inquiries
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminInquiryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminInquiryController> _logger;

        public AdminInquiryController(ApplicationDbContext context, ILogger<AdminInquiryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: AdminInquiry/Index
        public async Task<IActionResult> Index(string? filter)
        {
            var query = _context.Inquiries
                .Include(i => i.User)
                .Include(i => i.Property)
                .AsQueryable();

            // Apply filters
            query = filter switch
            {
                "unread" => query.Where(i => !i.IsRead),
                "read" => query.Where(i => i.IsRead),
                "resolved" => query.Where(i => i.IsResolved),
                "pending" => query.Where(i => !i.IsResolved),
                _ => query
            };

            var inquiries = await query
                .OrderByDescending(i => i.InquiryDate)
                .ToListAsync();

            ViewBag.Filter = filter;
            return View(inquiries);
        }

        // GET: AdminInquiry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquiry = await _context.Inquiries
                .Include(i => i.User)
                .Include(i => i.Property)
                .FirstOrDefaultAsync(m => m.InquiryId == id);

            if (inquiry == null)
            {
                return NotFound();
            }

            // Mark as read
            if (!inquiry.IsRead)
            {
                inquiry.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(inquiry);
        }

        // POST: AdminInquiry/Respond
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, string adminResponse)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            inquiry.AdminResponse = adminResponse;
            inquiry.ResponseDate = DateTime.Now;
            inquiry.IsResolved = true;
            inquiry.IsRead = true;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin responded to inquiry ID: {id}");
            TempData["Success"] = "Response submitted successfully!";

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: AdminInquiry/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            inquiry.IsRead = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminInquiry/ToggleResolved/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleResolved(int id)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            inquiry.IsResolved = !inquiry.IsResolved;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Inquiry marked as {(inquiry.IsResolved ? "resolved" : "pending")}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminInquiry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquiry = await _context.Inquiries
                .Include(i => i.User)
                .Include(i => i.Property)
                .FirstOrDefaultAsync(m => m.InquiryId == id);

            if (inquiry == null)
            {
                return NotFound();
            }

            return View(inquiry);
        }

        // POST: AdminInquiry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry != null)
            {
                _context.Inquiries.Remove(inquiry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Inquiry deleted: ID {id}");
                TempData["Success"] = "Inquiry deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
