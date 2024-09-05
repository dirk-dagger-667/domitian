using domition_api.Data.Identity;
using System.Security.Claims;

namespace dominitian.Business.Contracts
{
    public interface ITokenService
    {
        string GenerateJwt(DominitianIDUser user);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
