namespace domitian_api.Infrastructure.Constants
{
  public class AppConstants
  {
    public const string DomitianConnectionString = "DomitianDefaultConnectionString";
    public const string DomitianIntegrationTestsConnectionString = "DomitianIntegrationTestingConnectionString";
    public const string DBContextAssembly = "domitian.Data";

    public const string InnerKey = "Inner";
    public const string CrossCuttingKey = "Cross-cutting";
  }

  public class ControllerEndpPoints
  {
    public const string RegisterController = "Register";
    public const string ConfirmEmail = "ConfirmEmail";
  }
}
