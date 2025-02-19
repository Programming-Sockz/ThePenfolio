using System.ComponentModel.DataAnnotations;
using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Server.Models
{
    public class BookList
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public ListTypes ListType { get; set; }
    }
}
