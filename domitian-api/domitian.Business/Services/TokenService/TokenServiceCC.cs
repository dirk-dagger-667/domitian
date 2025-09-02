using domitian.Business.Contracts;
using domitian.Business.Extensions;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace domitian.Business.Services.TokenService
{
  public class TokenServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)] ITokenService inner,
    ILogger<TokenService> _logger) : ITokenService
  {
    public string GenerateJwt(DomitianIDUser user)
    {
      var result = inner.GenerateJwt(user);
      _logger.LogResult(result, nameof(GenerateJwt), nameof(ITokenService), user);

      return result;
    }

    public string GenerateRefreshToken()
    {
      var result = inner.GenerateRefreshToken();
      _logger.LogResult(result, nameof(GenerateRefreshToken), nameof(ITokenService), string.Empty);

      return result;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
      var result = inner.GetPrincipalFromExpiredToken(token);
      _logger.LogResult(result, nameof(GetPrincipalFromExpiredToken), nameof(ITokenService), $"{nameof(token)}: ***CENSURED***");

      return result;
    }
  }
}
