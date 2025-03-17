using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Interfaces;
using ThePenfolio.Server.Models;
namespace ThePenfolio.Server.Services
{
    public class ThePenfolioDbContext : DbContext, IThePenfolioDbContext
    {
        protected string Schema = "dbo";
        
        public DbSet<Book> Books { get; set; }
        public DbSet<BookBookList> BookBookList { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<BookList> BookList { get; set; }
        public DbSet<BookTags> BookTags { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChapterUserLikes> ChapterUserLikes { get; set; }
        public DbSet<UserBookReviews> UserBookReviews { get; set; }
        
        public ThePenfolioDbContext(DbContextOptions<ThePenfolioDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Chapters)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<BookGenre>(entity =>
            {
                entity.ToTable("BookGenre");
                entity.HasKey(e => new { e.BookId, e.GenreId });

                entity.HasOne(bg => bg.Book)
                    .WithMany(b => b.BookGenres)
                    .HasForeignKey(bg => bg.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bg => bg.Genre)
                    .WithMany(g => g.BookGenres)
                    .HasForeignKey(bg => bg.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookTags>(entity =>
            {
                entity.ToTable("BookTag");
                entity.HasKey(e => new { e.BookId, e.TagId });

                entity.HasOne(bg => bg.Book)
                    .WithMany(b => b.BookTags)
                    .HasForeignKey(bg => bg.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bg => bg.Tag)
                    .WithMany(g => g.BookTags)
                    .HasForeignKey(bg => bg.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookBookList>().HasKey(bbl => new
            {
                bbl.BookId,
                bbl.BookListId
            });
            
            modelBuilder.Entity<UserBookReviews>(entity => {
                entity.HasKey(ubr => new { ubr.UserId, ubr.BookId });
                
                entity.HasOne(ubr => ubr.User)
                    .WithMany(u => u.UserBookReviews)
                    .HasForeignKey(ubr => ubr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ubr=>ubr.Book)
                    .WithMany(b => b.UserBookReviews)
                    .HasForeignKey(ubr => ubr.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<ChapterUserLikes>(entity => {
                entity.HasKey(e => new { e.ChapterId, e.UserId });

                entity.HasOne(u => u.User)
                    .WithMany(u => u.ChapterUserLikes)
                    .HasForeignKey(ubr => ubr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ubr => ubr.Chapter)
                    .WithMany(c=> c.ChapterUserLikes)
                    .HasForeignKey(ubr => ubr.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            if (!string.IsNullOrWhiteSpace(Schema))
            {
                modelBuilder.HasDefaultSchema(Schema);
            }
            base.OnModelCreating(modelBuilder);
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }
}
