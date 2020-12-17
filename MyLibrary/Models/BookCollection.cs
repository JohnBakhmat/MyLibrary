using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models {
    public class BookCollection {
        [Key]
        public int BookCollectionId { get; set; }
        public DateTime Date { get; set; }
        
        public User User { get; set; }
        public int UserId { get; set; }

        public BookObject Ukrainian { get; set; }
        public int UkrainianId { get; set; }

        public BookObject Algebra { get; set; }
        public int AlgebraId { get; set; }

        public BookObject Geometry { get; set; }
        public int GeometryId { get; set; }

        public BookObject English { get; set; }
        public int EnglishId { get; set; }

        public BookObject Russian { get; set; }
        public int RussianId { get; set; }

        public BookObject UkraineHistory { get; set; }
        public int UkraineHistoryId { get; set; }

        public BookObject WorldHistory { get; set; }
        public int WorldHistoryId { get; set; }
        
        public BookObject Chemistry { get; set; }
        public int ChemistryId { get; set; }
        
        public BookObject Biology { get; set; }
        public int BiologyId { get; set; }
        
        public BookObject Informatics { get; set; }
        public int InformaticsId { get; set; }
        
        public BookObject Music { get; set; }
        public int MusicId { get; set; }
        
        public BookObject Literature { get; set; }
        public int LiteratureId { get; set; }
    }
}