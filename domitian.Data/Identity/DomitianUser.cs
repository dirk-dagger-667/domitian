using Microsoft.AspNetCore.Identity;

namespace domitian_api.Data.Identity
{
    public class DomitianIDUser: IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
