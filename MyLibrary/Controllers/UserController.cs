using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers {
    public class UserController : Controller {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index() {
            if (DateTime.Today.Month == 9 && DateTime.Today.Day == 1) NewSchoolYear();
            return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create() {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,FathersName,Age,Class,Rating")]
            User user) {
            if (ModelState.IsValid) {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,FathersName,Age,Class,Rating")]
            User user) {
            if (id != user.UserId) return NotFound();

            if (ModelState.IsValid) {
                try {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!UserExists(user.UserId))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => e.UserId == id);
        }

        public async Task<FileContentResult> Inventory() {
            var content = await _context.Users.ToListAsync();
            try {
                var buffer = "FirstName,FathersName,LastName,Age,Class,Rating";
                foreach (var user in content)
                    buffer +=
                        $"\n{user.FirstName},{user.FathersName},{user.LastName},{user.Age},{user.Class},{user.Rating}";
                return File(Encoding.UTF8.GetBytes(buffer), "text/csv", "users.csv");
            }
            catch {
                return null;
            }
        }

        public async void NewSchoolYear() {
            var users = await _context.Users.Where(u => !u.Class.Equals(0)).ToListAsync();
            if (users == null) return;
            foreach (var user in users) {
                user.Class++;
                if (user.Class.Equals(12)) user.Class = 0;
            }

            _context.Users.UpdateRange(users);
            RedirectToAction("Index");
        }
    }
}