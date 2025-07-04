using domitian.Infrastructure.Configuration.Authentication;
using domitian_api.Constants;
using Microsoft.Extensions.Options;

namespace domitian.Tests.Integration.Helpers
{
  public class ApiUrlPathBuilder(IOptionsMonitor<ApiUrlOptions> _options)
  {
    #region Login
    public string Login => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Login}";
    public string Refresh => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Login}/{ApiPathConstants.Refresh}";
    public string Revoke => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Login}/{ApiPathConstants.Revoke}";
    #endregion

    #region Register
    public string Register => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Register}";
    public string ConfirmEmail => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Register}/{ApiPathConstants.ConfirmEmail}";
    public string ConfirmRegistration => $"{_options.CurrentValue.ApiUrlBase}/{ApiPathConstants.Register}/{ApiPathConstants.ConfirmRegistration}";
    #endregion
  }
}
