using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class BookCategory {
        [Key]
        public int BookCategoryId { get; set; }
        public int BookId { get; set; }
        public int CategoryId { get; set; }
        public Book Book { get; set; }
        public Category Category { get; set; }
    }
}