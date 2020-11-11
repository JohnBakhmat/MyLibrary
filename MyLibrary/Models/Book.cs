using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models {
    public class Book {
        [Key]
        public int BookId { get; set; }
        
        public string Name { get; set; }
        
        public string ISBN { get; set; }
        
        public string Publisher { get; set; }
        
        public BookType BookType { get; set; }

        public string Language { get; set; }

        public int Ration { get; set; } = 0;

        public int Cost { get; set; } = 0;
        
        public ICollection<BookAuthor> Authors { get; set; } = new List<BookAuthor>();
        
        public ICollection<BookUser> UserLog { get; set; } = new List<BookUser>();
        
        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
        
        public string Image { get; set; }
    }
}