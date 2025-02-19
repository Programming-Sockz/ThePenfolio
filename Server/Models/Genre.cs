using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Server.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookGenre>? BookGenres { get; set; }
    }
}
