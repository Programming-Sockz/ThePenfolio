namespace ThePenfolio.Server.Models
{
    public class UserBookReviews
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid BookId { get; set; }
        public Book? Book { get; set; }
        
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
