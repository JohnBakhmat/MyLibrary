using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index(string searchString="", string sortBy = "name") {
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, 
                    b => b.BookId, 
                    ab => ab.BookId, 
                    (b, gj) => new {b, gj})
                .SelectMany(@t => @t.gj.DefaultIfEmpty(),
                    (@t, ab) => new BookAuthorViewModel() {
                        BookType = @t.b.BookType,
                        Name = @t.b.Name,
                        ISBN = @t.b.ISBN,
                        Cost = @t.b.Cost,
                        Language = @t.b.Language,
                        Publisher = @t.b.Publisher,
                        Author = ab.Author.FirstName + " " + ab.Author.LastName,
                        BookId = @t.b.BookId,
                        Image = @t.b.Image,
                        Rating = @t.b.Ration
                    });
            if (!string.IsNullOrEmpty(searchString)) {
                var sS = searchString.Split(' ');
                content = sS.Aggregate(content, (current, parameter) => current.Where(b => b.Author.Contains(parameter) || b.Name.Contains(parameter) || b.ISBN.Contains(parameter) || b.Publisher.Contains(parameter) || b.Language.Contains(parameter)));
            }

            content = sortBy switch {
                "name" => content.OrderBy(b => b.Name).ThenBy(b=>b.Author),
                "author" => content.OrderBy(b => b.Author).ThenBy(b=>b.Name),
                "lang" => content.OrderBy(b => b.Language),
                "isbn" => content.OrderBy(b => b.ISBN),
                "publisher" => content.OrderBy(b => b.Publisher),
                "type" => content.OrderBy(b => b.BookType),
                "rating" => content.OrderBy(b => b.Rating),
                _ => content
            };
            return View(await content.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var content = 
                from b in _context.Books
                join ab in _context.BookAuthors 
                    on b.BookId equals ab.BookId into gj
                from ab in gj.DefaultIfEmpty()
                select new BookAuthorViewModel() {
                    BookType = b.BookType, Name = b.Name, ISBN = b.ISBN, Cost = b.Cost, Language = b.Language,
                    Publisher = b.Publisher, Author = ab.Author.FirstName + " " + ab.Author.LastName, BookId = b.BookId, Image = b.Image, Rating = b.Ration
                };
            var book = await content.FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Name,ISBN,Publisher,BookType,Language,Ration,Cost,Image,Count")] Book book)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < book.Count; i++) {
                    var bo = new BookObject();
                    int code;
                    code = 1 + int.Parse(_context.BookObjects.Max(b => b.BookCode))+i;
                    bo.BookInfo = book;
                    bo.BookCode = code.ToString();
                    _context.Add(bo);
                }
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);
            Console.WriteLine(book.Name);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Name,ISBN,Publisher,BookType,Language,Ration,Cost,Image")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var content = 
                from b in _context.Books
                join ab in _context.BookAuthors 
                    on b.BookId equals ab.BookId into gj
                from ab in gj.DefaultIfEmpty()
                select new BookAuthorViewModel() {
                    BookType = b.BookType, Name = b.Name, ISBN = b.ISBN, Cost = b.Cost, Language = b.Language,
                    Publisher = b.Publisher, Author = ab.Author.FirstName + " " + ab.Author.LastName, BookId = b.BookId, Image = b.Image, Rating = b.Ration
                };
            var book = await content
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            var ba = await _context.BookAuthors.Where(b => b.BookId == id).ToListAsync();
            var bo = await _context.BookObjects.Where(b => b.BookInfo.BookId == id).ToListAsync();
            foreach (var item in ba) {
                _context.BookAuthors.Remove(item);
            }
            foreach (var item in bo) {
                _context.BookObjects.Remove(item);
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        public async Task<FileContentResult> Inventory () {
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, b => b.BookId, ab => ab.BookId, (b, gj) => new {b, gj})
                .SelectMany(@t => @t.gj.DefaultIfEmpty(),
                    (@t, ab) => new BookAuthorViewModel() {
                        BookType = @t.b.BookType,
                        Name = @t.b.Name,
                        ISBN = @t.b.ISBN,
                        Cost = @t.b.Cost,
                        Language = @t.b.Language,
                        Publisher = @t.b.Publisher,
                        Author = ab.Author.FirstName + " " + ab.Author.LastName,
                        BookId = @t.b.BookId,
                        Image = @t.b.Image,
                        Rating = @t.b.Ration
                    });
            try {
                var buffer = "Название,\tАвтор,\tТип,\tИздатель,\tISBN,\tЯзык,Рейтинг,Цена";
                foreach (var b in content) {
                    buffer += $"\n{b.Name},\t{b.Author},\t{b.BookType},\t{b.Publisher},\t{b.ISBN},\t{b.Language},\t{b.Rating},{b.Cost}";
                }
                return File(Encoding.UTF8.GetBytes(buffer), "text/csv", "books.csv");
            }
            catch {
                return null;
            }

        }

        // public IActionResult BookSet() {
        //     return View();
        // }
        //
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult BookSet() {
        //     
        // }
    }
}
