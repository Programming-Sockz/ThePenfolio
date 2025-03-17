namespace ThePenfolio.Shared.DTOs
{
    public class UserBookReviewsDTO
    {
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public Guid BookId { get; set; }
        public BookDTO? Book { get; set; }
        
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
