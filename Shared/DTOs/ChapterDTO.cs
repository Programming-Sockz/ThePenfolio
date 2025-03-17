using System.ComponentModel.DataAnnotations;
using Mapster;
namespace ThePenfolio.Shared.DTOs
{
    public class ChapterDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public string? AuthorNoteTop { get; set; }
        public string? AuthorNoteBottom { get; set; }
        [AdaptIgnore]
        public BookDTO? Book { get; set; }
    }
}
