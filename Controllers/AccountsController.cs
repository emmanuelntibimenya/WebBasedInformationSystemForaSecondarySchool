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
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Principal")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;

        public AccountsController(ApplicationDbContext context, UserManager<User> userManager, IUserStore<User> userStore)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<User>)_userStore;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.User'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRole = userRoles.FirstOrDefault();

            return View(user);
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Subjects = _context.Subject.ToList();
            ViewBag.Parents = await _userManager.GetUsersInRoleAsync("Parent");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,PhoneNumber,Password,Role,Subjects,Parent")] NewUser user)
        {
            if (ModelState.IsValid)
            {
                User newUser = Activator.CreateInstance<User>();

                newUser.FullName = user.FullName;
                newUser.Email = user.Email;
                newUser.PhoneNumber = user.PhoneNumber;
                newUser.EmailConfirmed = true;
                newUser.ParentId = !string.IsNullOrEmpty(user.Parent) ? user.Parent : null;
                
                await _userStore.SetUserNameAsync(newUser, newUser.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(newUser, newUser.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    var userRole = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.Role);

                    await _userManager.AddToRoleAsync(newUser, userRole?.Name);

                    List<UserSubject> subjects = new List<UserSubject>();

                    foreach (var subjectId in user.Subjects)
                    {
                        subjects.Add(new UserSubject
                        {
                            SubjectId = subjectId,
                            UserId = newUser.Id,
                            LearnerProfile = String.Empty
                        });
                    }
                    await _context.UserSubjects.AddRangeAsync(subjects);

                    await _context.SaveChangesAsync();

                }

                
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = _context.Roles.ToList();
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Subjects = _context.Subject.ToList();
            ViewBag.Parents = await _userManager.GetUsersInRoleAsync("Parent");

            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;

            NewUser newUser = new NewUser
            {
                Id = user.Id,
                FullName = user?.FullName,
                Email = user?.Email,
                PhoneNumber = user?.PhoneNumber,
                Role = userRoles.FirstOrDefault(),
                Parent = user?.ParentId
            };

            return View(newUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Email,PhoneNumber,Role,Subjects,Parent")] NewUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _context.Users.FindAsync(user.Id);
                    currentUser!.FullName = user.FullName;
                    currentUser.Email = user.Email;
                    currentUser.UserName = user.Email;
                    currentUser.PhoneNumber = user.PhoneNumber;
                    _context.Users.Update(currentUser);

                    var userRole = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.Role);

                    await _userManager.UpdateAsync(currentUser);
                    var oldRole = await _userManager.GetRolesAsync(currentUser);
                    if (oldRole != null && oldRole.Any())
                    {
                        await _userManager.RemoveFromRoleAsync(currentUser, oldRole.First());
                    }

                    await _userManager.AddToRoleAsync(currentUser, userRole?.Name);

                    if(userRole!.Name == "Student")
                    {
                        var userSubjects = await _context.UserSubjects.Where(x => x.UserId == currentUser.Id).ToListAsync();
                        List<UserSubject> subjects = new List<UserSubject>();
                        foreach (var subjectId in user.Subjects)
                        {
                            if (!userSubjects.Any(x => x.SubjectId == subjectId))
                            {
                                subjects.Add(new UserSubject
                                {
                                    SubjectId = subjectId,
                                    UserId = currentUser.Id,
                                    LearnerProfile = String.Empty
                                });
                            }
                        }
                        await _context.UserSubjects.AddRangeAsync(subjects);

                        currentUser.ParentId = !string.IsNullOrEmpty(user.Parent) ? user.Parent : null;
                        _context.Users.Update(currentUser);
                    }
                    

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewBag.Roles = _context.Roles.ToList();
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
