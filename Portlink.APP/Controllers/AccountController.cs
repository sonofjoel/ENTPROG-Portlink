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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View(model);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email is already taken";
                return View(model);
            }

        
            var user = new User
            {
                CompanyName = model.CompanyName,
                Email = model.Email,
                PasswordHash = model.Password, 
                Designation = model.Designation,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! You can now login.";
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

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

                switch (user.Designation.ToLower())
                {
                    case "trucker":
                        return RedirectToAction("TruckerDashboard", "Dashboard");
                    case "port":
                        return RedirectToAction("PortDashboard", "Dashboard");
                    case "warehouse":
                        return RedirectToAction("WarehouseDashboard", "Dashboard");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Wrong email or password";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}

