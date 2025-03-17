namespace ThePenfolio.Shared.DTOs
{
    public class ChapterUserLikesDTO
    {
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public Guid ChapterId { get; set; }
        public ChapterDTO? Chapter { get; set; }
    }
}
