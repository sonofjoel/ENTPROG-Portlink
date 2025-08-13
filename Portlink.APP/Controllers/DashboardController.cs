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
            var userType = HttpContext.Session.GetString("UserType");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(userType) || userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Redirect to role-specific dashboard
            switch (userType.ToLower())
            {
                case "port":
                    return RedirectToAction("PortDashboard");
                case "trucker":
                    return RedirectToAction("TruckerDashboard");
                case "warehouse":
                    return RedirectToAction("WarehouseDashboard");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        // Port Authority Dashboard
        public async Task<IActionResult> PortDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get port's schedules and bookings
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Where(b => b.Schedule.Port.PortName.Contains("Port")) // Adjust this logic
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            ViewBag.UserType = "Port";
            ViewBag.TotalBookings = bookings.Count;
            ViewBag.PendingBookings = bookings.Count(b => b.Status == "Pending");

            return View("PortDashboard", bookings);
        }

        // Trucker Dashboard
        public async Task<IActionResult> TruckerDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get trucker's bookings
            var myBookings = await _context.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Where(b => b.UserID == userId.Value)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            // Get available schedules
            var availableSchedules = await _context.Schedules
                .Include(s => s.Port)
                .Where(s => s.Status == "Available" && s.AvailableDate >= DateTime.Today)
                .OrderBy(s => s.AvailableDate)
                .Take(10)
                .ToListAsync();

            ViewBag.UserType = "Trucker";
            ViewBag.TotalBookings = myBookings.Count;
            ViewBag.AvailableSlots = availableSchedules.Count;
            ViewBag.AvailableSchedules = availableSchedules;

            return View("TruckerDashboard", myBookings);
        }

        // Warehouse Dashboard
        public async Task<IActionResult> WarehouseDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get warehouse-related bookings
            var warehouseBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .Where(b => b.DropOff.Contains("Warehouse") || b.Status == "In Transit")
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            ViewBag.UserType = "Warehouse";
            ViewBag.IncomingDeliveries = warehouseBookings.Count(b => b.Status == "Confirmed");
            ViewBag.InTransit = warehouseBookings.Count(b => b.Status == "In Transit");

            return View("WarehouseDashboard", warehouseBookings);
        }
    }
}
