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
    [Authorize(Roles = "Secretary,Principal")]
    public class AdministrationTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdministrationTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdministrationTasks
        public async Task<IActionResult> Index()
        {
              return _context.AdministrationTasks != null ? 
                          View(await _context.AdministrationTasks.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.AdministrationTasks'  is null.");
        }

        // GET: AdministrationTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdministrationTasks == null)
            {
                return NotFound();
            }

            var administrationTask = await _context.AdministrationTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrationTask == null)
            {
                return NotFound();
            }

            return View(administrationTask);
        }

        // GET: AdministrationTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdministrationTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Time,Task")] AdministrationTask administrationTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrationTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrationTask);
        }

        // GET: AdministrationTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdministrationTasks == null)
            {
                return NotFound();
            }

            var administrationTask = await _context.AdministrationTasks.FindAsync(id);
            if (administrationTask == null)
            {
                return NotFound();
            }
            return View(administrationTask);
        }

        // POST: AdministrationTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Time,Task")] AdministrationTask administrationTask)
        {
            if (id != administrationTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrationTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrationTaskExists(administrationTask.Id))
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
            return View(administrationTask);
        }

        // GET: AdministrationTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdministrationTasks == null)
            {
                return NotFound();
            }

            var administrationTask = await _context.AdministrationTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrationTask == null)
            {
                return NotFound();
            }

            return View(administrationTask);
        }

        // POST: AdministrationTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdministrationTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AdministrationTasks'  is null.");
            }
            var administrationTask = await _context.AdministrationTasks.FindAsync(id);
            if (administrationTask != null)
            {
                _context.AdministrationTasks.Remove(administrationTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministrationTaskExists(int id)
        {
          return (_context.AdministrationTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
