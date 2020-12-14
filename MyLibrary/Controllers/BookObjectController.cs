using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers {
    public class BookObjectController : Controller {
        private readonly ApplicationDbContext _context;

        public BookObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET
        public async Task<IActionResult> Index(int? id) {
            if (id == null)
            {
                return NotFound();
            }

            var context = _context.BookObjects
                .GroupJoin(_context.Books, bo => bo.BookInfo.BookId, b => b.BookId, (bo, b_bo) => new {bo, b_bo})
                .SelectMany(@t => @t.b_bo.DefaultIfEmpty(),
                    (@t, b) => new BookObjectViewModel() {
                        Name = b.Name, BookId = b.BookId, BookNumber = @t.bo.BookCode
                    }).Where(b=> b.BookId==id);
            return View( await context.ToListAsync());
        }
        
    }
}