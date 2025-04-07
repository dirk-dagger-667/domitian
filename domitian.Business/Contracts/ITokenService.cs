using domitian_api.Data.Identity;
using System.Security.Claims;

namespace domitian.Business.Contracts
{
    public interface ITokenService
    {
        string GenerateJwt(DomitianIDUser user);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
