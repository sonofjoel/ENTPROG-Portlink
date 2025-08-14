using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portlink.DataModel;

namespace Portlink.APP.Controllers
{
    public class PortController : Controller
    {
        private readonly AppDbContext _context;

        public PortController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ports = await _context.Ports.ToListAsync();
            return View(ports);
        }

        public async Task<IActionResult> Details(int id)
        {
            var port = await _context.Ports
                .Include(p => p.Schedules)
                .FirstOrDefaultAsync(p => p.PortID == id);
            return View(port);

            if (port == null)
            {
                return NotFound();
            }

            return View(port);
        }

    }
}
