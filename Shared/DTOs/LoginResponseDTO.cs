namespace ThePenfolio.Shared.DTOs
{
    public class LoginResponseDTO
    {
        public Guid? LoginStamp { get; set; }
        public Guid? UserId { get; set; }
        public bool Success { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}
