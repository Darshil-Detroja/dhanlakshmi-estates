using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Home page displaying featured properties
    /// </summary>
    public async Task<IActionResult> Index()
    {
        // Get featured properties first, then fill with regular properties if needed
        var featuredProperties = await _context.Properties
            .Include(p => p.PropertyImages)
            .Where(p => p.IsFeatured && p.Status == "Available")
            .OrderByDescending(p => p.CreatedDate)
            .Take(6)
            .ToListAsync();

        // If we don't have enough featured properties, add regular ones
        if (featuredProperties.Count < 6)
        {
            var featuredIds = featuredProperties.Select(p => p.PropertyId).ToList();
            var regularProperties = await _context.Properties
                .Include(p => p.PropertyImages)
                .Where(p => p.Status == "Available" && !featuredIds.Contains(p.PropertyId))
                .OrderByDescending(p => p.CreatedDate)
                .Take(6 - featuredProperties.Count)
                .ToListAsync();

            featuredProperties.AddRange(regularProperties);
        }

        return View(featuredProperties);
    }

    /// <summary>
    /// About page
    /// </summary>
    public IActionResult About()
    {
        return View();
    }

    /// <summary>
    /// Contact page
    /// </summary>
    public IActionResult Contact()
    {
        return View();
    }

    /// <summary>
    /// Privacy policy page
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Error page
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
