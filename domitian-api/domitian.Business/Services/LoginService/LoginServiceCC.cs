using domitian.Business.Contracts;
using domitian.Business.Constants;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian.Models.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using domitian_api.Infrastructure.Constants;

namespace domitian.Business.Services.LoginService
{
  public class LoginServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)]ILoginService inner,
    ILogger<LoginService> logger) : ILoginService
  {
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(LoginAsync), nameof(ILoginService), loginRequest);
      var result = await inner.LoginAsync(loginRequest);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(LoginAsync), nameof(ILoginService), result);

      return result;
    }

    public async Task<Result<LoginResponse>> RefreshAccessAsync(RefreshRequest refReq)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(RefreshAccessAsync), nameof(ILoginService), refReq);
      var result = await inner.RefreshAccessAsync(refReq);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(RefreshAccessAsync), nameof(ILoginService), result);

      return result;
    }

    public async Task<Result> RevokeAccessAsync(string? username)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(RevokeAccessAsync), nameof(ILoginService), username);
      var result = await inner.RevokeAccessAsync(username);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(RevokeAccessAsync), nameof(ILoginService), result);

      return result;
    }
  }
}
