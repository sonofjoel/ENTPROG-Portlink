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

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Show registration page
        public IActionResult Register()
        {
            return View();
        }

        // POST: Handle registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View(model);
            }

            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email is already taken";
                return View(model);
            }

            // Create new user
            var user = new User
            {
                CompanyName = model.CompanyName,
                Email = model.Email,
                PasswordHash = model.Password, // Consider hashing the password
                Designation = model.Designation,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! You can now login.";
            return RedirectToAction("Login");
        }

        // GET: Show login page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", user.CompanyName);
                HttpContext.Session.SetString("UserType", user.Designation);

                TempData["SuccessMessage"] = $"Welcome back, {user.CompanyName}!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Wrong email or password";
                return View();
            }
        }

        // Logout user
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
