using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class BookObject {
        [Key] public int BookObjectId { get; set; }
        
        public Book BookInfo { get; set; }
        public int BookInfoId { get; set; }
        public Shelf Shelf { get; set; }

        public string BookCode { get; set; }

        public ICollection<BookUser> UserLog { get; set; } = new List<BookUser>();
    }
}