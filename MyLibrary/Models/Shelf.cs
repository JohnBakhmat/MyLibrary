using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class Shelf {
        [Key]
        public int ShelfId { get; set; }
        
        public string BookcaseDescription { get; set; }
        
        public string ShelfCode { get; set; }
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}