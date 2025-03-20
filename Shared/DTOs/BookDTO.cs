using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Shared.DTOs
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage="Please provide a title")]
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        
        public Guid AuthorId { get; set; }
        
        public UserDTO? Author { get; set; }
        [MinLength(1, ErrorMessage="Please select at least one genre")]
        public ICollection<GenreDTO>? Genres { get; set; }
        [MinLength(1, ErrorMessage="Please add at least one tag")]
        public ICollection<TagDTO>? Tags { get; set; }
        public ICollection<ChapterDTO>? Chapters { get; set; }
        public ICollection<UserBookReviewsDTO>? UserBookReviews { get; set; }
    }
}
