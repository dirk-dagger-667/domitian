namespace domitian_api.Constants
{
  public class ApiPathConstants
  {
    #region Login
    public const string Login = "api/login";
    public const string Refresh = "refresh";
    public const string Revoke = "revoke";
    public const string RevokeControllerPath = $"{Revoke}" + "/{email}";
    #endregion

    #region Register
    public const string Register = "api/register";
    public const string ConfirmEmail = $"confirm-email";
    public const string ConfirmRegistration = $"confirm-registration";
    #endregion
  }
}
