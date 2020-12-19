using System;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class BookUser {
        [Key] public int Id { get; set; }

        public int BookId { get; set; }

        public DateTime Date { get; set; }

        public bool IsReturned { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public BookObject Book { get; set; }
    }
}