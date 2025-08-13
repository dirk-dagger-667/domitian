using domitian.Business.Constants;
using domitian.Business.Contracts;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace domitian.Business.Services.TokenService
{
  public class TokenServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)]ITokenService inner,
    ILogger<TokenService> logger) : ITokenService
  {
    public string GenerateJwt(DomitianIDUser user)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(GenerateJwt), user);
      var result = inner.GenerateJwt(user);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(GenerateJwt), result);

      return result;
    }

    public string GenerateRefreshToken()
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(GenerateRefreshToken), null);
      var result = inner.GenerateRefreshToken();
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(GenerateRefreshToken), result);

      return result;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(GetPrincipalFromExpiredToken), token);
      var result = inner.GetPrincipalFromExpiredToken(token);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(GetPrincipalFromExpiredToken), result);

      return result;
    }
  }
}
