using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Admin controller for managing properties
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminPropertyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminPropertyController> _logger;
        private readonly IWebHostEnvironment _environment;

        public AdminPropertyController(
            ApplicationDbContext context,
            ILogger<AdminPropertyController> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        // GET: AdminProperty/Index
        public async Task<IActionResult> Index()
        {
            var properties = await _context.Properties
                .Include(p => p.PropertyImages)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(properties);
        }

        // GET: AdminProperty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.Inquiries)
                .FirstOrDefaultAsync(m => m.PropertyId == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // GET: AdminProperty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminProperty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Property property, List<IFormFile>? images)
        {
            if (ModelState.IsValid)
            {
                property.CreatedDate = DateTime.Now;
                property.ViewCount = 0;

                _context.Add(property);
                await _context.SaveChangesAsync();

                // Handle image uploads
                if (images != null && images.Count > 0)
                {
                    await UploadPropertyImages(property.PropertyId, images);
                }

                _logger.LogInformation($"Property created: {property.Title} (ID: {property.PropertyId})");
                TempData["Success"] = "Property created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }

        // GET: AdminProperty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // POST: AdminProperty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Property property, List<IFormFile>? images)
        {
            if (id != property.PropertyId)
            {
                return NotFound();
            }

            // Remove validation errors for navigation properties
            ModelState.Remove("PropertyImages");
            ModelState.Remove("Inquiries");

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing property from database
                    var existingProperty = await _context.Properties
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.PropertyId == id);

                    if (existingProperty == null)
                    {
                        return NotFound();
                    }

                    // Preserve CreatedDate and ViewCount from existing property
                    property.CreatedDate = existingProperty.CreatedDate;
                    property.ViewCount = existingProperty.ViewCount;
                    property.ModifiedDate = DateTime.Now;

                    _context.Update(property);
                    await _context.SaveChangesAsync();

                    // Handle new image uploads
                    if (images != null && images.Count > 0)
                    {
                        await UploadPropertyImages(property.PropertyId, images);
                    }

                    _logger.LogInformation($"Property updated: {property.Title} (ID: {property.PropertyId})");
                    TempData["Success"] = "Property updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.PropertyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we got here, something failed, reload the property with images
            var propertyWithImages = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.PropertyId == id);
            
            return View(propertyWithImages ?? property);
        }

        // GET: AdminProperty/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(property);
        }

        // POST: AdminProperty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property != null)
            {
                // Delete associated images from filesystem
                if (property.PropertyImages != null)
                {
                    foreach (var image in property.PropertyImages)
                    {
                        DeleteImageFile(image.ImageUrl);
                    }
                }

                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Property deleted: {property.Title} (ID: {property.PropertyId})");
                TempData["Success"] = "Property deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminProperty/DeleteImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.PropertyImages.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var propertyId = image.PropertyId;
            DeleteImageFile(image.ImageUrl);

            _context.PropertyImages.Remove(image);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Image deleted successfully!";
            return RedirectToAction(nameof(Edit), new { id = propertyId });
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }

        /// <summary>
        /// Upload property images
        /// </summary>
        private async Task UploadPropertyImages(int propertyId, List<IFormFile> images)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            int displayOrder = _context.PropertyImages.Count(i => i.PropertyId == propertyId) + 1;
            bool isFirst = displayOrder == 1;

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var uniqueFileName = $"{propertyId}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    var propertyImage = new PropertyImage
                    {
                        PropertyId = propertyId,
                        ImageUrl = $"/images/properties/{uniqueFileName}",
                        IsPrimary = isFirst,
                        DisplayOrder = displayOrder++,
                        UploadedDate = DateTime.Now
                    };

                    _context.PropertyImages.Add(propertyImage);
                    isFirst = false;
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete image file from filesystem
        /// </summary>
        private void DeleteImageFile(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/images/"))
            {
                var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error deleting image file: {filePath}");
                    }
                }
            }
        }
    }
}
