namespace ThePenfolio.Shared.DTOs
{
    public class BookSearchDTO
    {
        public string Title { get; set; } = "";
        public Guid AuthorId { get; set; }
        public List<Guid> IncludeTags { get; set; } = new List<Guid>();
        public List<Guid> ExcludeTags { get; set; } = new List<Guid>();
        public List<Guid> IncludeGenres { get; set; } = new List<Guid>();
        public List<Guid> ExcludeGenres { get; set; } = new List<Guid>();
        
    }
}
