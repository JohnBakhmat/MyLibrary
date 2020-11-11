using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MyLibrary.Models {
    public class Category {
        [Key]
        public int CategoryId { get; set; }
        public CategoryLable Lable { get; set; }
        public string Color { get; set; }
        public int Rating { get; set; } = 0;
        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
    }
}