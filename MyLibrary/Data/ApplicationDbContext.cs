using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using MyLibrary.Models;

namespace MyLibrary.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookObject> BookObjects { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookCollection> BookCollections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>().HasKey(t => new {t.AuthorId, t.BookId});
            builder.Entity<BookCategory>().HasKey(t => new {t.CategoryId, t.BookId});
            builder.Entity<BookUser>().HasKey(t => new {t.UserId, t.BookId});

            builder.Entity<BookCategory>()
                .HasOne(sc => sc.Book)
                .WithMany(s => s.Categories)
                .HasForeignKey(sc => sc.BookId);
            builder.Entity<BookCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(s => s.Books)
                .HasForeignKey(sc => sc.CategoryId);
            /***Users***/
            builder.Entity<BookUser>()
                .HasOne(sc => sc.Book)
                .WithMany(s => s.UserLog)
                .HasForeignKey(sc => sc.BookId);
            builder.Entity<BookUser>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.BookLog)
                .HasForeignKey(sc => sc.UserId);

            builder.Entity<BookObject>()
                .HasOne(bo => bo.BookInfo)
                .WithMany(b => b.BookObjects);

            // Authors
            builder.Entity<BookAuthor>()
                .HasOne(sc => sc.Book)
                .WithMany(s => s.Authors)
                .HasForeignKey(sc => sc.BookId);
            builder.Entity<BookAuthor>()
                .HasOne(sc => sc.Author)
                .WithMany(s => s.Books)
                .HasForeignKey(sc => sc.AuthorId);
        }
    }
}