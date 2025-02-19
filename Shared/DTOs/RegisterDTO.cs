using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Shared.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
