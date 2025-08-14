using domitian.Business.Constants;
using domitian.Business.Contracts;
using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Services.RegisterService
{
  public class RegisterServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)]IRegisterService inner,
    ILogger<RegisterService> logger) : IRegisterService
  {
    public Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(ConfirmEmailAsync), nameof(RegisterServiceCC), request);
      var result = inner.ConfirmEmailAsync(request);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(ConfirmEmailAsync), nameof(RegisterServiceCC), result);

      return result;
    }

    public Task<Result<string>> ConfirmRegistrationAsync(string email)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(ConfirmRegistrationAsync), nameof(RegisterServiceCC), email);
      var result = inner.ConfirmRegistrationAsync(email);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(ConfirmRegistrationAsync), nameof(RegisterServiceCC), result);

      return result;
    }

    public Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
      logger.LogInformation(Messages.ExecStartTemplate, nameof(RegisterAsync), nameof(RegisterServiceCC), request);
      var result = inner.RegisterAsync(request);
      logger.LogInformation(Messages.ExectFinishTemplate, nameof(RegisterAsync), nameof(RegisterServiceCC), result);

      return result;
    }
  }
}
