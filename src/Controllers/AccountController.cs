using Microsoft.AspNetCore.Mvc;
using TEST2.Models;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace TEST2.Controllers
{
    public class AccountController : Controller
    {
        private readonly YourDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(YourDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult LoginSignup()
        {
            return View(new loginsignup());
        }
        [HttpPost]
        public IActionResult Login(loginsignup model)
        {
            if (ModelState.IsValid && !model.IsSignup)
            {
                // Hash the password (replace with actual hashing)
                var passwordHash = HashPassword(model.Password);

                // Validate user by email and hashed password
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == passwordHash);

                if (existingUser != null)
                {
                    // Successful login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid email or password.");
                }
            }

            // Set the active tab to "login" and return the view with the model
            ViewBag.ActiveTab = "login";
            return View("LoginSignup", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(loginsignup model)
        {
            // Set the IsSignup property to true for the signup process
            model.IsSignup = true;

            if (ModelState.IsValid && model.IsSignup)
            {
                try
                {
                    // Hash the password
                    var passwordHash = HashPassword(model.Password);

                    // Create a new user with signup details
                    var newUser = new Users
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PasswordHash = passwordHash,
                        Phone = model.Phone,
                        UserType = "Customer", // Default user type for signups
                        Address = model.Address,
                        City = model.City,
                        PostalCode = model.PostalCode
                    };

                    // Add and save the user to the database
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    // Redirect to home page after successful signup
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Log the exception and add error message to ModelState
                    _logger.LogError($"Error during signup: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Error during signup.");
                }
            }

            // Set the active tab to "signup" and return the view
            ViewBag.ActiveTab = "signup";
            return View("LoginSignup", model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
            return password; // Placeholder
        }
    }
}
