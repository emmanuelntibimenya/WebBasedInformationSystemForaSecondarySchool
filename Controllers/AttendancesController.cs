using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class AttendancesController:Controller
    {
        private readonly ApplicationDbContext _context;
       

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances

        public async Task<IActionResult> Index()
        {
            return _context.LearnerProfile != null ?
                          View(await _context.Attendance.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Attendance'  is null.");
        }

       

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id, object attendance)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var Attendance = await _context.Attendance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Note")] Attendance Attendance)
        {
           

            if (ModelState.IsValid)
            {
                _context.Add(Attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var Attendance = await _context.Attendance.FindAsync(id);
            if (Attendance == null)
            {
                return NotFound();
            }
            return View(Attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Note")] Attendance Attendance)
        {
            if (id != Attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(Attendance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var Attendance = await _context.Attendance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Attendance == null)
            {
                return NotFound();
            }

            return View(Attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendance == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Attendance'  is null.");
            }
            var Attendance = await _context.Attendance.FindAsync(id);
            if (Attendance != null)
            {
                _context.Attendance.Remove(Attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return (_context.Attendance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
    

