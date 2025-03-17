namespace ThePenfolio.Server.Models
{
    public class ChapterUserLikes
    {
        public Guid ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
