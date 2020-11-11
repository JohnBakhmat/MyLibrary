using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class User {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public short Age { get; set; }
        public short Class { get; set; } = 0;
        public int Rating { get; set; } = 0;
        public ICollection<BookUser> BookLog { get; set; } = new List<BookUser>();
    }
}