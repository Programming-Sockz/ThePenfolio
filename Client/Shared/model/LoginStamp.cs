using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Client.Shared.model
{
    public class LoginStamp
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        public static string LoginStampStorageKey = "loginStamp";

        public LoginStamp(LoginResponseDTO loginResponse)
        {
            Id = loginResponse.LoginStamp.Value;
            UserId = loginResponse.UserId.Value;
        }
        public LoginStamp(){} // For deserialization (fucking cunt)
    }
}
