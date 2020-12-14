namespace MyLibrary.Models.ViewModels {
    public class BookObjectViewModel {
        public int BookId { get; set; }
        public string Name { get; set; }
        
        public int AuthorId { get; set; }
        
        public string BookNumber { get; set; }

        public int LastUsersId { get; set; }
    }
}