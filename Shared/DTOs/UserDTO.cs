namespace ThePenfolio.Shared.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public DateTime DateJoined { get; set; }
        public string Bio { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        
        public ICollection<BookDTO>? Books { get; set; }
        public ICollection<UserBookReviewsDTO>? UserBookReviewsDTO { get; set; }
        public ICollection<ChapterUserLikesDTO>? ChapterUserLikesDTO { get; set; }
    }
}
