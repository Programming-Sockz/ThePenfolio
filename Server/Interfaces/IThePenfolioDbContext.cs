using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
namespace ThePenfolio.Server.Interfaces
{
    public interface IThePenfolioDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookBookList> BookBookList { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<BookList> BookList { get; set; }
        public DbSet<BookTags> BookTags { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
