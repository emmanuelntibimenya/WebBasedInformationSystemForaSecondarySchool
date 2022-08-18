using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Parent,Principal")]
    [Authorize]
    public class ParentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ParentsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Parent
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isPrincipal = await _userManager.IsInRoleAsync(user, "Principal");
            List<UserSubject> subjects = new List<UserSubject>();
            if (isPrincipal)
            {
                ViewBag.Heading = string.Empty;
                subjects = await _context.UserSubjects.Include(u => u.Subject).Include(x => x.User).OrderBy(x => x.UserId).ToListAsync();
            }
            else
            {
                ViewBag.Heading = $"{user.FullName}'s Childrens' Academic Records";
                var childrenIds = await _context.Users.Where(x => x.ParentId == user.Id).Select(x => x.Id).ToListAsync();
                subjects = await _context.UserSubjects.Where(x => childrenIds.Contains(x.UserId)).Include(u => u.Subject).Include(x => x.User).OrderBy(x => x.UserId).ToListAsync();
            }
            
            return View(subjects);
        }

        // GET: Parent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserSubjects == null)
            {
                return NotFound();
            }

            var userSubject = await _context.UserSubjects
                .Include(u => u.Subject)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSubject == null)
            {
                return NotFound();
            }

            return View(userSubject);
        }

        // GET: Parent/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Parent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SubjectId,Attended,Score,LearnerProfile")] UserSubject userSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Id", userSubject.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubject.UserId);
            return View(userSubject);
        }

        // GET: Parent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserSubjects == null)
            {
                return NotFound();
            }

            var userSubject = await _context.UserSubjects.FindAsync(id);
            if (userSubject == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Id", userSubject.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubject.UserId);
            return View(userSubject);
        }

        // POST: Parent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,SubjectId,Attended,Score,LearnerProfile")] UserSubject userSubject)
        {
            if (id != userSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSubjectExists(userSubject.Id))
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
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Id", userSubject.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubject.UserId);
            return View(userSubject);
        }

        // GET: Parent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserSubjects == null)
            {
                return NotFound();
            }

            var userSubject = await _context.UserSubjects
                .Include(u => u.Subject)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSubject == null)
            {
                return NotFound();
            }

            return View(userSubject);
        }

        // POST: Parent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserSubjects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserSubjects'  is null.");
            }
            var userSubject = await _context.UserSubjects.FindAsync(id);
            if (userSubject != null)
            {
                _context.UserSubjects.Remove(userSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSubjectExists(int id)
        {
            return (_context.UserSubjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
