using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Shared.DTOs
{
    public class BookListDTO
    {
        public Guid Id { get; set; }
        public ListTypes ListType { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<BookDTO> Books { get; set; }
    }
}
