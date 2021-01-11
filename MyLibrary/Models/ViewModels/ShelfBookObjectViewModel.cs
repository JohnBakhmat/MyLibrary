namespace MyLibrary.Models.ViewModels {
    public class ShelfBookObjectViewModel {
        public int ShelfId { get; set; }
        
        public string BookcaseDescription { get; set; }

        public string ShelfCode { get; set; }

        public int BooksCount { get; set; }
        
        public Category Category { get; set; }
        
        public string BookObjects { get; set;}
    }
}