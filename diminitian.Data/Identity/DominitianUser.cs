using Microsoft.AspNetCore.Identity;

namespace domition_api.Data.Identity
{
    public class DominitianIDUser: IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
