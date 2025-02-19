using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Server.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        
        public Guid AuthorId { get; set; }
        
        public User? Author { get; set; }
        public ICollection<BookTags>? BookTags { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
        public ICollection<Chapter>? Chapters { get; set; }
    }
}
