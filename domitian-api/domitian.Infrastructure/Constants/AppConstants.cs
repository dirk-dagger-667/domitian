namespace domitian_api.Infrastructure.Constants
{
  public class AppConstants
  {
    public const string DomitianConnectionString = "DomitianDefaultConnectionString";
    public const string DomitianIntegrationTestsConnectionString = "DomitianIntegrationTestingConnectionString";
    public const string dbContextAssembly = "domitian.Data";
  }

  public class ControllerEndpPoints
  {
    #region Register

    public const string RegisterController = "Register";
    public const string ConfirmEmail = "ConfirmEmail";

    #endregion

  }
}
