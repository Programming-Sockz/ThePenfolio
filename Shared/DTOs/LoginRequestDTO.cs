using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Shared.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
