using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class Shelf {
        [Key] public int ShelfId { get; set; }

        public string BookcaseDescription { get; set; }

        public string ShelfCode { get; set; }

        public int BooksCount { get; set; } = 0;

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<BookObject> Books { get; set; } = new List<BookObject>();
    }
}