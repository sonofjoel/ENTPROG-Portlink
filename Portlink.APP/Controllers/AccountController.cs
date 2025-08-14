using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portlink.DataModel;
using Portlink.APP.Models;
using System.Diagnostics;

namespace Portlink.APP.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to initialize the database context
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Display the Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Display the Registration page
        public IActionResult Register()
        {
            return View();
        }

        // POST: Handle Registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View(model);
            }

            // Check if the email already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email is already taken";
                return View(model);
            }

            // Create a new user object
            var user = new User
            {
                CompanyName = model.CompanyName,
                Email = model.Email,
                PasswordHash = model.Password, // WARNING: Ensure to hash passwords before storing
                Designation = model.Designation,
                CreatedAt = DateTime.Now
            };

            // Save the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Display success message and redirect to login page
            TempData["SuccessMessage"] = "Registration successful! You can now login.";
            return RedirectToAction("Login");
        }

        // GET: Show the Login page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Find user by email and password
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

            // Check if credentials are valid
            if (user != null)
            {
                // Set user-related session variables
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", user.CompanyName);
                HttpContext.Session.SetString("UserType", user.Designation);

                // Display welcome message and redirect to home page
                TempData["SuccessMessage"] = $"Welcome back, {user.CompanyName}!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Invalid credentials
                ViewBag.Error = "Wrong email or password";
                return View();
            }
        }

        // Logout action
        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
