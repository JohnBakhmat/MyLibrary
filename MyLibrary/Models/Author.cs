using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class Author {
        [Key]
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public int Rating { get; set; } = 0;
        public ICollection<BookAuthor> Books { get; set; } = new List<BookAuthor>();
    }
}