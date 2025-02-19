namespace ThePenfolio.Shared.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public DateTime DateJoined { get; set; }
        public string Bio { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
