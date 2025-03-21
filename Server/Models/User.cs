using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Server.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }
        public DateTime DateJoined { get; set; }
        public string Bio { get; set; } = "";
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        
        public ICollection<UserBookReviews>? UserBookReviews { get; set; }
        public ICollection<ChapterUserLikes>? ChapterUserLikes { get; set; }
    }
}
