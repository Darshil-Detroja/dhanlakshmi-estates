using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;
using RealEstateManagementSystem.Models.ViewModels;
using System.Security.Claims;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Handles property listing, search, and details
    /// </summary>
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(ApplicationDbContext context, ILogger<PropertyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Property/Index
        public async Task<IActionResult> Index(PropertySearchViewModel model)
        {
            // Build query
            var query = _context.Properties
                .Include(p => p.PropertyImages)
                .Where(p => p.Status == "Available")
                .AsQueryable();

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                query = query.Where(p =>
                    p.Title.Contains(model.SearchTerm) ||
                    p.Description.Contains(model.SearchTerm) ||
                    p.City.Contains(model.SearchTerm) ||
                    p.State.Contains(model.SearchTerm) ||
                    p.Address.Contains(model.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(model.PropertyType))
            {
                query = query.Where(p => p.PropertyType == model.PropertyType);
            }

            if (!string.IsNullOrWhiteSpace(model.City))
            {
                query = query.Where(p => p.City.Contains(model.City));
            }

            if (!string.IsNullOrWhiteSpace(model.State))
            {
                query = query.Where(p => p.State == model.State);
            }

            if (model.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= model.MinPrice.Value);
            }

            if (model.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= model.MaxPrice.Value);
            }

            if (model.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= model.MinBedrooms.Value);
            }

            if (model.MinBathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms >= model.MinBathrooms.Value);
            }

            // Count total results
            model.TotalResults = await query.CountAsync();

            // Apply sorting
            query = model.SortBy switch
            {
                "Price" => model.SortOrder == "asc" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "Title" => model.SortOrder == "asc" ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title),
                "CreatedDate" => model.SortOrder == "asc" ? query.OrderBy(p => p.CreatedDate) : query.OrderByDescending(p => p.CreatedDate),
                _ => query.OrderByDescending(p => p.CreatedDate)
            };

            // Apply pagination
            model.Properties = await query
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();

            // Get distinct values for filters
            ViewBag.PropertyTypes = await _context.Properties
                .Select(p => p.PropertyType)
                .Distinct()
                .ToListAsync();

            ViewBag.Cities = await _context.Properties
                .Select(p => p.City)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.States = await _context.Properties
                .Select(p => p.State)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            return View(model);
        }

        // GET: Property/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(m => m.PropertyId == id);

            if (property == null)
            {
                return NotFound();
            }

            // Increment view count
            property.ViewCount++;
            await _context.SaveChangesAsync();

            // Get related properties (same type, different property)
            var relatedProperties = await _context.Properties
                .Include(p => p.PropertyImages)
                .Where(p => p.PropertyType == property.PropertyType && p.PropertyId != property.PropertyId && p.Status == "Available")
                .OrderByDescending(p => p.IsFeatured)
                .Take(3)
                .ToListAsync();

            ViewBag.RelatedProperties = relatedProperties;

            return View(property);
        }

        // POST: Property/SubmitInquiry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInquiry(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                // Get user ID if authenticated
                if (User.Identity?.IsAuthenticated == true)
                {
                    inquiry.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                inquiry.InquiryDate = DateTime.Now;
                inquiry.IsRead = false;
                inquiry.IsResolved = false;

                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"New inquiry submitted by {inquiry.Email} for property {inquiry.PropertyId}");

                TempData["Success"] = "Your inquiry has been submitted successfully! We will contact you soon.";
                
                if (inquiry.PropertyId.HasValue)
                {
                    return RedirectToAction(nameof(Details), new { id = inquiry.PropertyId });
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "There was an error submitting your inquiry. Please try again.";
            
            if (inquiry.PropertyId.HasValue)
            {
                return RedirectToAction(nameof(Details), new { id = inquiry.PropertyId });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Property/Search
        public IActionResult Search()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
