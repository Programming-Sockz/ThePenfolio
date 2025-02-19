namespace ThePenfolio.Server.Models
{
    public class BookTags
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
