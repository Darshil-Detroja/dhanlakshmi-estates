using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models.ViewModels;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Admin dashboard controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminDashboardController> _logger;

        public AdminDashboardController(ApplicationDbContext context, ILogger<AdminDashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: AdminDashboard/Index
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalProperties = await _context.Properties.CountAsync(),
                AvailableProperties = await _context.Properties.CountAsync(p => p.Status == "Available"),
                SoldProperties = await _context.Properties.CountAsync(p => p.Status == "Sold"),
                TotalUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalInquiries = await _context.Inquiries.CountAsync(),
                UnreadInquiries = await _context.Inquiries.CountAsync(i => !i.IsRead),
                RecentInquiries = await _context.Inquiries.CountAsync(i => i.InquiryDate >= DateTime.Now.AddDays(-7)),
                FeaturedProperties = await _context.Properties.CountAsync(p => p.IsFeatured),
                RecentProperties = await _context.Properties
                    .Include(p => p.PropertyImages)
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(5)
                    .ToListAsync(),
                LatestInquiries = await _context.Inquiries
                    .Include(i => i.User)
                    .Include(i => i.Property)
                    .OrderByDescending(i => i.InquiryDate)
                    .Take(5)
                    .ToListAsync()
            };

            // Add pending profile updates count
            ViewBag.PendingUpdates = await _context.Users.CountAsync(u => u.HasPendingUpdates);

            return View(viewModel);
        }
    }
}
