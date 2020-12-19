using System;

namespace MyLibrary.Models.ViewModels {
    public class BookObjectViewModel {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string BookNumber { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
    }
}