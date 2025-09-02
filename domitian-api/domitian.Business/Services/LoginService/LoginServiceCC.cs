using domitian.Business.Contracts;
using domitian.Business.Extensions;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian.Models.Results;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Services.LoginService
{
  public class LoginServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)] ILoginService inner,
    ILogger<LoginService> _logger) : ILoginService
  {
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
      var result = await inner.LoginAsync(loginRequest);
      _logger.LogResult(result, nameof(LoginAsync), nameof(ILoginService), loginRequest);

      return result;
    }

    public async Task<Result<LoginResponse>> RefreshAccessAsync(RefreshRequest refReq)
    {
      var result = await inner.RefreshAccessAsync(refReq);
      _logger.LogResult(result, nameof(RefreshAccessAsync), nameof(ILoginService), refReq);

      return result;
    }

    public async Task<Result> RevokeAccessAsync(string? username)
    {
      var result = await inner.RevokeAccessAsync(username);
      _logger.LogResult(result, nameof(RevokeAccessAsync), nameof(ILoginService), $"{nameof(username)}: ***CENSURED***");

      return result;
    }
  }
}
