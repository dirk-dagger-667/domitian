using domitian.Business.Contracts;
using domitian.Business.Extensions;
using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Services.RegisterService
{
  public class RegisterServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)] IRegisterService inner,
    ILogger<RegisterService> _logger) : IRegisterService
  {
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
      var result = await inner.ConfirmEmailAsync(request);
      _logger.LogResult(result, nameof(ConfirmEmailAsync), nameof(IRegisterService), request);

      return result;
    }

    public async Task<Result<string>> ConfirmRegistrationAsync(string email)
    {
      var result = await inner.ConfirmRegistrationAsync(email);
      _logger.LogResult(result, nameof(ConfirmRegistrationAsync), nameof(IRegisterService), $"{nameof(email)}: ***CENSURED***");

      return result;
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
      var result = await inner.RegisterAsync(request);
      _logger.LogResult(result, nameof(RegisterAsync), nameof(IRegisterService), request);

      return result;
    }
  }
}
