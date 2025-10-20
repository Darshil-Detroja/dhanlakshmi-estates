using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;
using RealEstateManagementSystem.Models.ViewModels;
using RealEstateManagementSystem.Services;
using System.Security.Claims;

namespace RealEstateManagementSystem.Controllers
{
    /// <summary>
    /// Handles user authentication and account management
    /// </summary>
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ApplicationDbContext context,
            IAuthService authService,
            ILogger<AccountController> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Try to authenticate as User first
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && user.IsActive && _authService.VerifyPassword(model.Password, user.PasswordHash))
            {
                // Update last login
                user.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();

                // Sign in the user
                await _authService.SignInAsync(HttpContext, user.UserId, user.Email, user.FullName, "User", model.RememberMe);

                _logger.LogInformation($"User {user.Email} logged in successfully.");

                // Redirect to Home page for users
                return RedirectToAction("Index", "Home");
            }

            // Try to authenticate as Admin
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (admin != null && admin.IsActive && _authService.VerifyPassword(model.Password, admin.PasswordHash))
            {
                // Update last login
                admin.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();

                // Sign in the admin
                await _authService.SignInAsync(HttpContext, admin.AdminId, admin.Email, admin.Name, "Admin", model.RememberMe);

                _logger.LogInformation($"Admin {admin.Email} logged in successfully.");

                return RedirectToAction("Index", "AdminDashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // GET: Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if email already exists
            var existingUser = await _context.Users.AnyAsync(u => u.Email == model.Email);
            var existingAdmin = await _context.Admins.AnyAsync(a => a.Email == model.Email);

            if (existingUser || existingAdmin)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            // Create new user
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = _authService.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New user registered: {user.Email}");

            // Automatically sign in the user
            await _authService.SignInAsync(HttpContext, user.UserId, user.Email, user.FullName, "User", false);

            TempData["Success"] = "Registration successful! Welcome to Real Estate Management System.";
            return RedirectToAction("Index", "Home");
        }

        // POST: Account/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _authService.SignOutAsync(HttpContext);

            _logger.LogInformation($"User {userEmail} logged out.");

            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Please enter your email address.");
                return View();
            }

            // Check if email exists (User or Admin)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);

            if (user == null && admin == null)
            {
                // Don't reveal that the user doesn't exist
                ViewBag.ShowInstructions = true;
                ViewBag.Email = email;
                return View();
            }

            // Show success with detailed instructions
            _logger.LogInformation($"Password reset requested for email: {email}");
            
            ViewBag.ShowInstructions = true;
            ViewBag.Email = email;
            return View();
        }

        // GET: Account/Profile
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Profile
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId != model.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Store pending updates as JSON for admin approval
            var pendingChanges = new
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                RequestedDate = DateTime.Now
            };

            user.PendingUpdates = System.Text.Json.JsonSerializer.Serialize(pendingChanges);
            user.HasPendingUpdates = true;

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Profile update request submitted! Waiting for admin approval.";
                _logger.LogInformation($"User {user.Email} submitted profile update request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting profile update for user {user.Email}");
                TempData["Error"] = "An error occurred while submitting your profile update request.";
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}
