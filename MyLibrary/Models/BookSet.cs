using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class BookSet {
        [Key] public int BookSetId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<BookObject> Books { get; set; } = new List<BookObject>();
    }
}