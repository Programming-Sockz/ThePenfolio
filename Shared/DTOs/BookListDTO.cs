using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Shared.DTOs
{
    public class BookListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public UserDTO? CreatedBy { get; set; }
        public ListTypes ListType { get; set; }
        public List<BookDTO> Books { get; set; } = new();
    }
}
