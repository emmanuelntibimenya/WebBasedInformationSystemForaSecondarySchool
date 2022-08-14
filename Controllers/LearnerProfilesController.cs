using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher,Student,Parent,Principal")]
    public class LearnerProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LearnerProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LearnerProfiles
        public async Task<IActionResult> Index()
        {
              return _context.LearnerProfile != null ? 
                          View(await _context.LearnerProfile.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LearnerProfile'  is null.");
        }

        // GET: LearnerProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LearnerProfile == null)
            {
                return NotFound();
            }

            var learnerProfile = await _context.LearnerProfile
                .FirstOrDefaultAsync(m => m.ID == id);
            if (learnerProfile == null)
            {
                return NotFound();
            }

            return View(learnerProfile);
        }

        // GET: LearnerProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearnerProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Note")] LearnerProfile learnerProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learnerProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learnerProfile);
        }

        // GET: LearnerProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LearnerProfile == null)
            {
                return NotFound();
            }

            var learnerProfile = await _context.LearnerProfile.FindAsync(id);
            if (learnerProfile == null)
            {
                return NotFound();
            }
            return View(learnerProfile);
        }

        // POST: LearnerProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Note")] LearnerProfile learnerProfile)
        {
            if (id != learnerProfile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnerProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerProfileExists(learnerProfile.ID))
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
            return View(learnerProfile);
        }

        // GET: LearnerProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LearnerProfile == null)
            {
                return NotFound();
            }

            var learnerProfile = await _context.LearnerProfile
                .FirstOrDefaultAsync(m => m.ID == id);
            if (learnerProfile == null)
            {
                return NotFound();
            }

            return View(learnerProfile);
        }

        // POST: LearnerProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LearnerProfile == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LearnerProfile'  is null.");
            }
            var learnerProfile = await _context.LearnerProfile.FindAsync(id);
            if (learnerProfile != null)
            {
                _context.LearnerProfile.Remove(learnerProfile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerProfileExists(int id)
        {
          return (_context.LearnerProfile?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
