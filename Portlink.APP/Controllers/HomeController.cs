using Microsoft.AspNetCore.Mvc;
using Portlink.APP.Models;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // New method to show the registration page
    public IActionResult Register()
    {
        return View();
    }

    // Handle registration form submission
    [HttpPost]
    public async Task<IActionResult> Register(UserModel model) // Assuming you have a UserModel for registration
    {
        if (ModelState.IsValid)
        {
            // Logic to save the user data 
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect after successful registration
        }
        return View(model); // Return view with model data if validation fails
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
