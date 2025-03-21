using Mapster;

namespace ThePenfolio.Shared.DTOs
{
    public class ChapterUserLikesDTO
    {
        public Guid UserId { get; set; }
        [AdaptIgnore]
        public UserDTO? User { get; set; }
        public Guid ChapterId { get; set; }
        [AdaptIgnore]
        public ChapterDTO? Chapter { get; set; }
    }
}
