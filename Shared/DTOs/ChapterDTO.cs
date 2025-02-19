using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Shared.DTOs
{
    public class ChapterDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string? AuthorNoteTop { get; set; }
        public string? AuthorNoteBottom { get; set; }
    }
}
