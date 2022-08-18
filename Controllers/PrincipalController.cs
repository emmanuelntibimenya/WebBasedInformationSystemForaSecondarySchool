using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrincipalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Principal
        public async Task<IActionResult> Index()
        {
              return _context.Diaries != null ? 
                          View(await _context.Diaries.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Diaries'  is null.");
        }

        // GET: Principal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Diaries == null)
            {
                return NotFound();
            }

            var diary = await _context.Diaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diary == null)
            {
                return NotFound();
            }

            return View(diary);
        }

        // GET: Principal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Principal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Time,Note")] Diary diary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diary);
        }

        // GET: Principal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Diaries == null)
            {
                return NotFound();
            }

            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }
            return View(diary);
        }

        // POST: Principal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Time,Note")] Diary diary)
        {
            if (id != diary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaryExists(diary.Id))
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
            return View(diary);
        }

        // GET: Principal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Diaries == null)
            {
                return NotFound();
            }

            var diary = await _context.Diaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diary == null)
            {
                return NotFound();
            }

            return View(diary);
        }

        // POST: Principal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Diaries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Diaries'  is null.");
            }
            var diary = await _context.Diaries.FindAsync(id);
            if (diary != null)
            {
                _context.Diaries.Remove(diary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiaryExists(int id)
        {
          return (_context.Diaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
