using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portlink.DataModel;

namespace Portlink.APP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (userId == null || userType == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Redirect to appropriate dashboard based on user role
            switch (userType.ToLower())
            {
                case "trucker":
                    return RedirectToAction("TruckerDashboard");
                case "port":
                    return RedirectToAction("PortDashboard");
                case "warehouse":
                    return RedirectToAction("WarehouseDashboard");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> TruckerDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get available deliveries for trucker
            var availableBookings = await _context.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Include(b => b.User)
                .Where(b => b.Status == "Confirmed")
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            var truckerBookings = await _context.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Where(b => b.UserID == userId.Value)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.AvailableDeliveries = availableBookings;
            ViewBag.MyBookings = truckerBookings;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }

        public async Task<IActionResult> PortDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get all bookings for port overview
            var todaysBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Where(b => b.BookingDate.Date == DateTime.Today)
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            var ports = await _context.Ports.ToListAsync();

            ViewBag.TodaysBookings = todaysBookings;
            ViewBag.TotalBookings = todaysBookings.Count;
            ViewBag.PendingBookings = todaysBookings.Count(b => b.Status == "Pending");
            ViewBag.Ports = ports;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }

        public async Task<IActionResult> WarehouseDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get warehouse orders and tracking
            var warehouseBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Include(b => b.Tracking)
                .Where(b => b.UserID == userId.Value)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.MyOrders = warehouseBookings;
            ViewBag.PendingOrders = warehouseBookings.Count(b => b.Status == "Pending");
            ViewBag.CompletedOrders = warehouseBookings.Count(b => b.Status == "Completed");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }
    }
}

