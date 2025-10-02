using domitian.Models.Requests.Users;

namespace domitian.Tests.Infrastructure.DataSources.UserAdmin.Endpoints
{
  public class PutEndpointsTestData
  {
    public static IEnumerable<object[]> ValidationFailureSingleTestData
    {
      get
      {
        yield return new object[] { new UpdateUserRequest(null, null, null, null, null) };
        yield return new object[] { new UpdateUserRequest("fake", "fake", null, null, null) };
        yield return new object[] { new UpdateUserRequest("fake", null, string.Empty, null, null) };
        yield return new object[] { new UpdateUserRequest("fake", null, null, string.Empty, null) };
        yield return new object[] { new UpdateUserRequest("fake", null, null, null, "fake") };
        yield return new object[] { new UpdateUserRequest("fake", "fake", null, null, "fake") };
      }
    }

    public static IEnumerable<object[]> ValidationFailureBulkTestData
    {
      get
      {
        yield return new object[]
        {
          new List<UpdateUserRequest>()
          {
            new UpdateUserRequest(null, null, null, null, null),
            new UpdateUserRequest(null, null, null, null, null),
          }
        };

        yield return new object[]
        {
          new List<UpdateUserRequest>()
          {
            new UpdateUserRequest("fake", null, null, null, null),
            new UpdateUserRequest(null, null, null, null, null),
          }
        };

        yield return new object[]
        {
          new List<UpdateUserRequest>()
          {
            new UpdateUserRequest("fake", null, null, null, null),
            new UpdateUserRequest("fake", null, string.Empty, string.Empty, null),
          }
        };

        yield return new object[] { new List<UpdateUserRequest>() { } };
      }
    }
  }
}
