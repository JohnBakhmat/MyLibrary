using System.Collections.Generic;

namespace MyLibrary.Models.ViewModels {
    public class BookAuthorViewModel {
        public int BookId { get; set; }
        
        public string Name { get; set; }
        
        public string ISBN { get; set; }
        
        public string Publisher { get; set; }
        
        public BookType BookType { get; set; }

        public string Language { get; set; }

        public double Cost { get; set; } = 0;

        public string Author { get; set; }
        
        public string Image { get; set; }
        
        public int Rating { get; set; }
    }
}