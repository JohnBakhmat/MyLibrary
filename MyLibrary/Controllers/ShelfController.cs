using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers {
    public class ShelfController : Controller {
        private readonly ApplicationDbContext _context;

        public ShelfController(ApplicationDbContext context) {
            _context = context;
        }
        // GET
        public async Task<IActionResult> Index() {
            var content =
                from s in _context.Shelves.Include(shelf => shelf.Category)
                join o in _context.BookObjects on s.ShelfId equals o.Shelf.ShelfId
                    into so
                from item in so.DefaultIfEmpty()
                select new ShelfBookObjectViewModel() {
                    ShelfId = s.ShelfId,
                    ShelfCode = s.ShelfCode,
                    Category = s.Category,
                    BookcaseDescription = s.BookcaseDescription,
                    BookObjects = item.BookCode != null ? $"{item.BookCode}" : " ",
                };
            var content_v2 = content.AsEnumerable().GroupBy(item => new {
                item.Category,
                item.ShelfId,
                item.BookcaseDescription,
                item.ShelfCode,
            }).Select(grp => new ShelfBookObjectViewModel() {
                ShelfId = grp.Key.ShelfId,
                ShelfCode = grp.Key.ShelfCode,
                BookcaseDescription = grp.Key.BookcaseDescription,
                Category = grp.Key.Category,
                BookObjects = string.Join("\n", grp.Select(ee => ee.BookObjects).ToList()),
                BooksCount = 0
            });
            return View(content_v2.ToList());
        }

        // GET: Category/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShelfId,BookcaseDescription,ShelfCode,BooksCount,CategoryId")]
            Shelf shelf) {
            if (ModelState.IsValid) {
                _context.Add(shelf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shelf);
        }
        
    }
}