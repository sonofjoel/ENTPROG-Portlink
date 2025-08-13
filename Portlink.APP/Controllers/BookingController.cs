using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portlink.DataModel;

namespace Portlink.APP.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Port)
                .ToListAsync();
            return View(bookings);
        }

        public IActionResult Create()
        {
            ViewBag.Ports = _context.Ports.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int scheduleId, string dropOff)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = new Booking
            {
                UserID = userId.Value,
                ScheduleID = scheduleId,
                DropOff = dropOff,
                BookingDate = DateTime.Now,
                Status = "Confirmed"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
