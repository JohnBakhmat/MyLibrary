using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers {
    public class BookUserController : Controller {
        private readonly ApplicationDbContext _context;

        public BookUserController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: BookAuthor
        public async Task<IActionResult> Index() {
            var content = from bo in _context.BookObjects
                join b in _context.Books on bo.BookInfo.BookId equals b.BookId
                join bu in _context.BookUsers on bo.BookObjectId equals bu.BookId
                join u in _context.Users on bu.UserId equals u.UserId
                select new BookObjectViewModel {
                    Name = b.Name,
                    BookNumber = bo.BookCode,
                    BookId = b.BookId,
                    User = $"{u.LastName} {u.FirstName} {u.FathersName}",
                    DateTime = bu.Date,
                    UserId = u.UserId
                };
            return View(await content.ToListAsync());
        }

        // GET: BookAuthor/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var bookAuthor = await _context.BookUsers
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (bookAuthor == null) return NotFound();

            return View(bookAuthor);
        }

        // GET: BookAuthor/Create
        public async Task<IActionResult> Create() {
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Books"] = await _context.BookObjects.ToListAsync();
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["BookId"] = new SelectList(_context.BookObjects, "BookId", "BookId");
            return View();
        }
        public async Task<IActionResult> Create2(string bookCode) {
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Books"] = _context.BookObjects.Where(o => o.BookCode == bookCode).ToList();
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["BookId"] = new SelectList(_context.BookObjects, "BookId", "BookId");
            return View("Create");
        }
        // POST: BookAuthor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId")] BookUser bookUser) {
            var a = await _context.BookUsers.ToListAsync();
            var id = a.Any() ? 1 + a.OrderByDescending(i => i.Id).First().Id : 1;
            bookUser.Id = id;

            if (ModelState.IsValid) {
                var bookObject = await _context.BookObjects.FirstOrDefaultAsync(bo => bo.BookObjectId== bookUser.BookId);
                var user = await _context.Users.FirstOrDefaultAsync(u=>bookUser.UserId == u.UserId);
                var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookObject.BookInfoId);
                var bookAuthors = _context.BookAuthors.Where(ba => ba.BookId == book.BookId);
                foreach (var pair in bookAuthors) {
                    (await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == pair.AuthorId)).Rating++;
                }
                bookUser.Book = bookObject;
                bookUser.User = user;
                user.Rating++;
                book.Ration++;
                user.BookLog.Add(bookUser);
                bookObject.UserLog.Add(bookUser);
                Console.WriteLine(bookObject.UserLog.Count);
                Console.WriteLine(user.BookLog.Count);
                _context.Add(bookUser);
                _context.Update(user);
                _context.Update(bookObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Books"] = await _context.Books.ToListAsync();
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", bookUser.UserId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookUser.BookId);
            return View(bookUser);
        }

        // GET: BookAuthor/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var bookUser = await _context.BookUsers.FirstOrDefaultAsync(b => b.Id.Equals(id));
            if (bookUser == null) return NotFound();
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", bookUser.UserId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookUser.BookId);
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Books"] = await _context.BookObjects.ToListAsync();
            return View(bookUser);
        }

        // POST: BookAuthor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId")] BookUser bookUser) {
            if (ModelState.IsValid) {
                var ba = await _context.BookUsers.FirstAsync(item => item.Book.BookInfo.BookId == bookUser.Id);
                var book = await _context.BookObjects.FirstOrDefaultAsync(bo => bo.BookInfo.BookId == bookUser.BookId);
                var author = await _context.Users.FindAsync(bookUser.UserId);
                book.UserLog.Remove(ba);
                author.BookLog.Remove(ba);
                _context.BookUsers.Remove(ba);
                author.BookLog.Add(bookUser);
                book.UserLog.Add(bookUser);
                _context.BookUsers.Add(bookUser);
                _context.Update(author);
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", bookUser.UserId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookUser.BookId);
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Books"] = await _context.BookObjects.ToListAsync();
            return View(bookUser);
        }

        // GET: BookAuthor/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();

            var bookAuthor = await _context.BookUsers
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookAuthor == null) return NotFound();

            return View(bookAuthor);
        }

        // POST: BookAuthor/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var ba = await _context.BookUsers.FirstAsync(item => item.Id == id);
            var book = await _context.BookObjects.FindAsync(ba.BookId);
            var author = await _context.Users.FindAsync(ba.UserId);
            book.UserLog.Remove(ba);
            author.BookLog.Remove(ba);
            _context.BookUsers.Remove(ba);
            _context.Update(author);
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAuthorExists(int id) {
            return _context.BookUsers.Any(e => e.Id == id);
        }
    }
}