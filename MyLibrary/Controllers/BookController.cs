using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers {
    public class BookController : Controller {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Book
        public IActionResult Index(string searchString = "", string sortBy = "name") {
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, b => b.BookId, ab => ab.BookId,
                    (b, gj) => new {b, gj}).SelectMany(
                    t => t.gj.DefaultIfEmpty(),
                    (t, ab) => new BookAuthorViewModel {
                        BookType = t.b.BookType,
                        Name = t.b.Name,
                        ISBN = t.b.ISBN,
                        Cost = t.b.Cost,
                        Language = t.b.Language,
                        Publisher = t.b.Publisher,
                        Author = ab.Author != null ? $"{ab.Author.LastName} {ab.Author.FirstName.First()};" : " ",
                        BookId = t.b.BookId,
                        Image = t.b.Image,
                        Rating = t.b.Ration
                    });
            var contentV2 = content.AsEnumerable().GroupBy(item => new {
                item.BookId,
                item.BookType,
                item.Name,
                item.ISBN,
                item.Publisher,
                item.Cost,
                item.Language,
                item.Image,
                item.Rating
            }).Select(grp => new BookAuthorViewModel {
                BookId = grp.Key.BookId,
                BookType = grp.Key.BookType,
                Name = grp.Key.Name,
                ISBN = grp.Key.ISBN,
                Publisher = grp.Key.Publisher,
                Cost = grp.Key.Cost,
                Language = grp.Key.Language,
                Image = grp.Key.Image,
                Rating = grp.Key.Rating,
                Author = string.Join("\n", grp.Select(ee => ee.Author).OrderBy(a => a.First()).ToList())
            });
            if (!string.IsNullOrEmpty(searchString)) {
                var sS = searchString.Split(' ');
                contentV2 = sS.Aggregate(contentV2,
                    (current, parameter) => current.Where(b =>
                        b.Author.Contains(parameter) || b.Name.Contains(parameter) || b.ISBN.Contains(parameter) ||
                        b.Publisher.Contains(parameter) || b.Language.Contains(parameter)));
            }

            contentV2 = sortBy switch {
                "name" => contentV2.OrderBy(b => b.Name).ThenBy(b => b.Author),
                "author" => contentV2.OrderBy(b => b.Author).ThenBy(b => b.Name),
                "lang" => contentV2.OrderBy(b => b.Language),
                "isbn" => contentV2.OrderBy(b => b.ISBN),
                "publisher" => contentV2.OrderBy(b => b.Publisher),
                "type" => contentV2.OrderBy(b => b.BookType),
                "rating" => contentV2.OrderBy(b => b.Rating),
                _ => contentV2
            };
            return View(contentV2);
        }

        // GET: Book/Details/5
        public IActionResult Details(int? id) {
            if (id == null) return NotFound();
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, b => b.BookId, ab => ab.BookId,
                    (b, gj) => new {b, gj}).SelectMany(
                    t => t.gj.DefaultIfEmpty(),
                    (t, ab) => new BookAuthorViewModel {
                        Author = ab.Author != null ? $"{ab.Author.LastName} {ab.Author.FirstName.First()};" : " ",
                        BookId = t.b.BookId,
                        BookType = t.b.BookType,
                        Cost = t.b.Cost,
                        ISBN = t.b.ISBN,
                        Image = t.b.Image,
                        Language = t.b.Language,
                        Name = t.b.Name,
                        Publisher = t.b.Publisher,
                        Rating = t.b.Ration
                    });
            var c2 = content.AsEnumerable().GroupBy(item => new {
                item.BookId,
                item.BookType,
                item.Cost,
                item.ISBN,
                item.Image,
                item.Language,
                item.Name,
                item.Publisher,
                item.Rating
            }).Select(grp => new BookAuthorViewModel {
                BookId = grp.Key.BookId,
                BookType = grp.Key.BookType,
                Name = grp.Key.Name,
                ISBN = grp.Key.ISBN,
                Publisher = grp.Key.Publisher,
                Cost = grp.Key.Cost,
                Language = grp.Key.Language,
                Image = grp.Key.Image,
                Rating = grp.Key.Rating,
                Author = string.Join("\n", grp.Select(ee => ee.Author).OrderBy(a => a.First()).ToList())
            });
            var book = c2.FirstOrDefault(m => m.BookId == id);
            return View(book);
        }
        // GET: Book/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("BookId,Name,ISBN,Publisher,BookType,Language,Ration,Cost,Image,Count")]
            Book book) {
            if (!ModelState.IsValid) return View(book);
            var code = int.Parse(_context.BookObjects.Select(bo => bo.BookCode).Last());
            for (var i = 0; i < book.Count; i++) {
                code++;
                var bo = new BookObject();
                bo.BookCode = $"{code}";
                bo.BookInfo = book;
                _context.Add(bo);
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();
            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);
            Console.WriteLine(book.Name);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("BookId,Name,ISBN,Publisher,BookType,Language,Ration,Cost,Image")]
            Book book) {
            if (id != book.BookId) return NotFound();
            if (ModelState.IsValid) {
                try {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!BookExists(book.BookId)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, b => b.BookId, ab => ab.BookId,
                    (b, gj) => new {b, gj}).SelectMany(
                    t => t.gj.DefaultIfEmpty(),
                    (t, ab) => new BookAuthorViewModel {
                        BookType = t.b.BookType,
                        Name = t.b.Name,
                        ISBN = t.b.ISBN,
                        Cost = t.b.Cost,
                        Language = t.b.Language,
                        Publisher = t.b.Publisher,
                        Author = ab.Author != null ? $"{ab.Author.LastName} {ab.Author.FirstName.First()};" : " ",
                        BookId = t.b.BookId,
                        Image = t.b.Image,
                        Rating = t.b.Ration
                    });
            var c2 = content.AsEnumerable().GroupBy(item => new {
                item.BookId,
                item.BookType,
                item.Name,
                item.ISBN,
                item.Publisher,
                item.Cost,
                item.Language,
                item.Image,
                item.Rating
            }).Select(grp => new BookAuthorViewModel {
                BookId = grp.Key.BookId,
                BookType = grp.Key.BookType,
                Name = grp.Key.Name,
                ISBN = grp.Key.ISBN,
                Publisher = grp.Key.Publisher,
                Cost = grp.Key.Cost,
                Language = grp.Key.Language,
                Image = grp.Key.Image,
                Rating = grp.Key.Rating,
                Author = string.Join("\n", grp.Select(ee => ee.Author).OrderBy(a => a.First()).ToList())
            });
            var book = c2.FirstOrDefault(m => m.BookId == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var book = await _context.Books.FindAsync(id);
            var ba = await _context.BookAuthors.Where(b => b.BookId == id).ToListAsync();
            var bo = await _context.BookObjects.Where(b => b.BookInfo.BookId == id).ToListAsync();
            foreach (var item in ba) _context.BookAuthors.Remove(item);
            foreach (var item in bo) _context.BookObjects.Remove(item);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id) {
            return _context.Books.Any(e => e.BookId == id);
        }

        public async void Automatisation() {
            WhereToSet();
            RedirectToAction("Index", "Shelf");
        }
        //Automatisation
        public async void WhereToSet() {
            var books = await _context.Books.ToListAsync();
            foreach (var book in books) {
                var bookObjects = await _context.BookObjects.Where(bo=>bo.ShelfId==0).ToListAsync();
                if (bookObjects.Count == 0) continue;
                var category = await _context.BookCategories.FirstOrDefaultAsync(c => c.BookId == book.BookId);
                if (category == null) continue;
                var shelves = await _context.Shelves
                .Where(s => s.CategoryId == category.CategoryId)
                .ToListAsync();
                foreach (var bookObject in bookObjects) {
                    if(shelves.Count==0) break;
                    var current = shelves.First();
                    if (current.BooksCount < 24) {
                        current.BooksCount++;
                        bookObject.Shelf = current;
                        _context.Shelves.Update(current);
                        _context.BookObjects.Update(bookObject);
                    }
                    else {
                        shelves.Remove(current);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return;
        }

        public FileContentResult Inventory() {
            var content = _context.Books
                .GroupJoin(_context.BookAuthors, b => b.BookId, ab => ab.BookId, (b, gj) => new {b, gj}).SelectMany(
                    t => t.gj.DefaultIfEmpty(),
                    (t, ab) => new BookAuthorViewModel {
                        BookType = t.b.BookType,
                        Name = t.b.Name,
                        ISBN = t.b.ISBN,
                        Cost = t.b.Cost,
                        Language = t.b.Language,
                        Publisher = t.b.Publisher,
                        Author = $"{ab.Author.LastName} {ab.Author.FirstName};",
                        BookId = t.b.BookId,
                        Image = t.b.Image,
                        Rating = t.b.Ration,
                        BookCount = t.b.Count
                    });
            var contentGrouped = content.AsEnumerable()
                .GroupBy(item => new {
                    item.BookId,
                    item.BookType,
                    item.Name,
                    item.ISBN,
                    item.Publisher,
                    item.Cost,
                    item.Language,
                    item.Image,
                    item.Rating,
                    item.BookCount
                }).Select(grp => new BookAuthorViewModel {
                    BookId = grp.Key.BookId,
                    BookType = grp.Key.BookType,
                    Name = grp.Key.Name,
                    ISBN = grp.Key.ISBN,
                    Publisher = grp.Key.Publisher,
                    Cost = grp.Key.Cost,
                    Language = grp.Key.Language,
                    Image = grp.Key.Image,
                    Rating = grp.Key.Rating,
                    BookCount = grp.Key.BookCount,
                    Author = string.Join("\n", grp.Select(ee => ee.Author).OrderBy(a => a.First()).ToList())
                });
            try {
                var buffer = "Название\tАвтор\tТип\tИздатель\tISBN\tЯзык\tРейтинг\tЦена\tКолличество";
                var count = 0;
                foreach (var b in contentGrouped) {
                    count += b.BookCount;
                    buffer += $"\n{b.Name}\t{b.Author.Replace(";\n", ",")}\t{b.BookType}\t{b.Publisher}\t{b.ISBN}\t{b.Language}\t{b.Rating}\t{b.Cost}\t{b.BookCount}";
                }

                buffer += $"\n\t\t\t\t\t\t\tВсего изданий:\t{count}";
                return File(Encoding.UTF8.GetBytes(buffer), "text/csv", $"Books || {DateTime.Now:dddd, dd MMM yyyy}.csv");
            }
            catch {
                return null;
            }
        }
    }
}