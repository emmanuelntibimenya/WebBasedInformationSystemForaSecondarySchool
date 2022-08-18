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
    [Authorize(Roles = "Teacher,Principal")]
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeachersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(int subjectId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Subject? subject = await _context.Subject.FindAsync(subjectId);
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var studentIds = students.Select(x => x.Id).ToList();
            var subjectUsers = subject == null ? new List<UserSubject>() : await _context.UserSubjects.Include(a => a.User).Where(x => x.SubjectId == subjectId && studentIds.Contains(x.UserId)).ToListAsync();

            ViewBag.Subjects = await _context.Subject.ToListAsync();
            bool isPrincipal = await _userManager.IsInRoleAsync(currentUser, "Principal");
            if (isPrincipal)
            {
                ViewBag.Heading = string.Empty;
            }
            else
            {
                ViewBag.Heading = $"Class {currentUser.FullName}";
            }
            
            return View(subjectUsers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Secretary,Principal")]
        public async Task<IActionResult> Create([Bind("Id,Title")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

        [HttpGet("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance(int id)
        {
            UserSubject? userSubject = await _context.UserSubjects.Include(x => x.User).Include(x => x.Subject).FirstOrDefaultAsync(x => x.Id == id);
            return View(userSubject);
        }

        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance(int id, [Bind("UserId,SubjectId,Attended,Score,LearnerProfile")] UserSubject userSubject)
        {
            UserSubject? userSubj = await _context.UserSubjects.Include(x => x.User).Include(x => x.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (userSubj != null)
            {
                userSubj.Attended = userSubject.Attended;
                userSubj.Score = userSubject.Score;
                userSubj.LearnerProfile = userSubject.LearnerProfile;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MarkAttendance), new { id = id });
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Subject == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subject'  is null.");
            }
            var subject = await _context.Subject.FindAsync(id);
            if (subject != null)
            {
                _context.Subject.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int? id)
        {
          return (_context.Subject?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
