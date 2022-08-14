using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Constants;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Secretary,Parent,Principal")]
    public class MeetingRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public MeetingRequestsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MeetingRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MeetingRequests.Include(m => m.Parent).Include(m => m.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MeetingRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MeetingRequests == null)
            {
                return NotFound();
            }

            var meetingRequest = await _context.MeetingRequests
                .Include(m => m.Parent)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingRequest == null)
            {
                return NotFound();
            }

            return View(meetingRequest);
        }

        // GET: MeetingRequests/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var children = _context.Users.Where(x => x.ParentId == user.Id).ToList();

            ViewData["ParentId"] = new SelectList(_context.Users, "Id", "FullName", user.Id);
            ViewData["StudentId"] = new SelectList(children, "Id", "FullName");
            return View();
        }

        // POST: MeetingRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentId,StudentId,MeetingDate,Note")] MeetingRequest meetingRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meetingRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), controllerName:"Parents");
            }
            var children = _context.Users.Where(x => x.ParentId == meetingRequest.ParentId).ToList();
            ViewData["ParentId"] = new SelectList(_context.Users, "Id", "FullName", meetingRequest.ParentId);
            ViewData["StudentId"] = new SelectList(children, "Id", "FullName", meetingRequest.StudentId);
            return View(meetingRequest);
        }

        // GET: MeetingRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MeetingRequests == null)
            {
                return NotFound();
            }

            var meetingRequest = await _context.MeetingRequests.FindAsync(id);
            if (meetingRequest == null)
            {
                return NotFound();
            }
            var children = _context.Users.Where(x => x.ParentId == meetingRequest.ParentId).ToList();
            ViewData["ParentId"] = new SelectList(_context.Users, "Id", "Id", meetingRequest.ParentId);
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", meetingRequest.StudentId);
            return View(meetingRequest);
        }

        // POST: MeetingRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,StudentId,MeetingDate,Note")] MeetingRequest meetingRequest)
        {
            if (id != meetingRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetingRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingRequestExists(meetingRequest.Id))
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
            ViewData["ParentId"] = new SelectList(_context.Users, "Id", "Id", meetingRequest.ParentId);
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", meetingRequest.StudentId);
            return View(meetingRequest);
        }

        // GET: MeetingRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MeetingRequests == null)
            {
                return NotFound();
            }

            var meetingRequest = await _context.MeetingRequests
                .Include(m => m.Parent)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingRequest == null)
            {
                return NotFound();
            }

            return View(meetingRequest);
        }

        // POST: MeetingRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MeetingRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MeetingRequests'  is null.");
            }
            var meetingRequest = await _context.MeetingRequests.FindAsync(id);
            if (meetingRequest != null)
            {
                _context.MeetingRequests.Remove(meetingRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingRequestExists(int id)
        {
          return (_context.MeetingRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
