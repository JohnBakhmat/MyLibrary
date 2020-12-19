using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Data;
using MyLibrary.Models.ViewModels;

namespace MyLibrary.Controllers {
    public class BookObjectController : Controller {
        private readonly ApplicationDbContext _context;

        public BookObjectController(ApplicationDbContext context) {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index(int? id) {
            if (id == null) return NotFound();
            /* SELECT Name,BookCode, LastName,FirstName from BookObjects 
                JOIN Books B on B.BookId = BookObjects.BookInfoBookId
                JOIN BookUsers BU on BookObjects.BookObjectId = BU.BookId
                JOIN Users U on U.UserId = BU.UserId;*/
            var content = from bo in _context.BookObjects
                join b in _context.Books on bo.BookInfo.BookId equals b.BookId
                join bu in _context.BookUsers on bo.BookObjectId equals bu.BookId into bbu
                from bu in bbu.DefaultIfEmpty()
                join u in _context.Users on bu.UserId equals u.UserId into bbuu
                from u in bbuu.DefaultIfEmpty()
                select new BookObjectViewModel {
                    Name = b.Name,
                    BookNumber = bo.BookCode,
                    BookId = b.BookId,
                    User = $"{u.LastName} {u.FirstName} {u.FathersName}",
                    DateTime = bu.Date,
                    UserId = u.UserId
                };
            return View(await content.Where(b => b.BookId == id).ToListAsync());
        }

        public async void GetUser(string bookCode) { }
    }
}