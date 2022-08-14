using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Secretary,Principal")]
    public class SecretariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SecretariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MeetingRequests.Include(m => m.Parent).Include(m => m.Student);
            return View("~/Views/MeetingRequests/Index.cshtml", await applicationDbContext.ToListAsync());
        }
    }
}
