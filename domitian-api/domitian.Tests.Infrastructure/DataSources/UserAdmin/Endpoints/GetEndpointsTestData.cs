using domitian.Models.Dtos.UserAdmin;

namespace domitian.Tests.Infrastructure.DataSources.UserAdmin.Endpoints
{
  public class GetEndpointsTestData
  {
    public static IEnumerable<object[]> ValidationFailureTestData
    {
      get
      {
        yield return new object[] { new PaganationParameters(0, 50) };
        yield return new object[] { new PaganationParameters(1, 101) };
      }
    }
  }
}
