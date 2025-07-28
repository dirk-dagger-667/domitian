namespace domitian.Tests.Integration.Helpers
{
  public class SqlContainerTestConfig
  {
    public static string SectionName = "MsSqlServerTestContainer";

    public string? Database { get; set; }
  }
}
