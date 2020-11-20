using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BookAuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookAuthorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookAuthor
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookAuthors.Include(b => b.Author).Include(b => b.Book);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookAuthor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BookAuthors
                .Include(b => b.Author)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // GET: BookAuthor/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Authors"] = await _context.Authors.ToListAsync();
            ViewData["Books"] = await _context.Books.ToListAsync();
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId");
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId");
            return View();
        }

        // POST: BookAuthor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookAuthorId,BookId,AuthorId")] BookAuthor bookAuthor) {
            var a = await _context.BookAuthors.ToListAsync();
            var id =1+a.OrderByDescending(i => i.BookAuthorId).First().BookAuthorId;
            bookAuthor.BookAuthorId = id;
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(bookAuthor.BookId);
                var author = await _context.Authors.FindAsync(bookAuthor.AuthorId);
                bookAuthor.Book = book;
                bookAuthor.Author = author;
                author.Books.Add(bookAuthor);
                book.Authors.Add(bookAuthor);
                Console.WriteLine(book.Authors.Count);
                Console.WriteLine(author.Books.Count);
                _context.Add(bookAuthor);
                _context.Update(author);
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Authors"] = await _context.Authors.ToListAsync();
            ViewData["Books"] = await _context.Books.ToListAsync();
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", bookAuthor.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookAuthor.BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BookAuthors.FirstOrDefaultAsync(b => b.BookAuthorId.Equals(id));
            if (bookAuthor == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", bookAuthor.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookAuthor.BookId);
            ViewData["Authors"] = await _context.Authors.ToListAsync();
            return View(bookAuthor);
        }

        // POST: BookAuthor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookAuthorId,BookId,AuthorId")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid) {
                var ba =  await _context.BookAuthors.FirstAsync(item => item.BookAuthorId == bookAuthor.BookAuthorId);
                var book = await _context.Books.FindAsync(bookAuthor.BookId);
                var author = await _context.Authors.FindAsync(bookAuthor.AuthorId);
                book.Authors.Remove(ba);
                author.Books.Remove(ba);
                _context.BookAuthors.Remove(ba);
                author.Books.Add(bookAuthor);
                book.Authors.Add(bookAuthor);
                _context.BookAuthors.Add(bookAuthor);
                _context.Update(author);
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", bookAuthor.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", bookAuthor.BookId);
            ViewData["Authors"] = await _context.Authors.ToListAsync();
            return View(bookAuthor);
        }

        // GET: BookAuthor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BookAuthors
                .Include(b => b.Author)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BookAuthorId == id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // POST: BookAuthor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ba =  await _context.BookAuthors.FirstAsync(item => item.BookAuthorId == id);
            var book = await _context.Books.FindAsync(ba.BookId);
            var author = await _context.Authors.FindAsync(ba.AuthorId);
            book.Authors.Remove(ba);
            author.Books.Remove(ba);
            _context.BookAuthors.Remove(ba);
            _context.Update(author);
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAuthorExists(int id)
        {
            return _context.BookAuthors.Any(e => e.AuthorId == id);
        }
    }
}
