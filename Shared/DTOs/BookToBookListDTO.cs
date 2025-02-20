using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Shared.DTOs
{
    public class BookToBookListDTO
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public ListTypes ListType { get; set; }
    }
}
