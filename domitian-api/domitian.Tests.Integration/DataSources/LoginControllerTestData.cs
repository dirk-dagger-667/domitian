using domitian.Models.Requests.Registration;
using domitian.Tests.Integration.Helpers;
using System.Net;

namespace domitian.Tests.Integration.DataSources
{
  public class LoginControllerTestData
  {
    private static IEnumerable<RegisterRequest>? _validAccountSeedData;
    private static IEnumerable<RegisterRequest>? _validUnconfirmedAccountSeedData;

    public static IEnumerable<object[]> ExistingAccountRegisterRequests
    {
      get
      {
        if (_validAccountSeedData is null)
          _validAccountSeedData = ValidAccountSeedData;

        foreach (var request in _validAccountSeedData)
        {
          yield return new object[] { request };
        }
      }
    }

    public static IEnumerable<object[]> ErrorRevokeRequests
    {
      get
      {
        yield return new object[] { "fakeEmail@dummy.com", HttpStatusCode.Unauthorized, "https://tools.ietf.org/html/rfc9110#section-15.5.2" };
        yield return new object[] { "invalidEmail", HttpStatusCode.BadRequest, "https://tools.ietf.org/html/rfc9110#section-15.5.1" };
      }
    }

    public static IEnumerable<object[]> ExistingAccountRefreshRequestInvalidTokens
    {
      get
      {
        if (_validAccountSeedData is null)
          _validAccountSeedData = ValidAccountSeedData;

        var asList = _validAccountSeedData.ToList();

        yield return new object[]
        {
          asList[0],
          true,
          null
        };

        yield return new object[] { asList[1], false, "InvalidRefreshToken" };
      }
    }

    public static IEnumerable<RegisterRequest> ValidAccountSeedData
    {
      get
      {
        if (_validAccountSeedData is null)
          _validAccountSeedData = SeedDataGenerator.GetRegisterRequests(3, true);

        return _validAccountSeedData;
      }
    }

    public static IEnumerable<object[]> ExistingUncinfirmedAccountRegisterRequests
    {
      get
      {
        if (_validUnconfirmedAccountSeedData is null)
          _validUnconfirmedAccountSeedData = ValidUnconfirmedAccountSeedData;

        foreach (var request in _validUnconfirmedAccountSeedData)
        {
          yield return new object[] { request };
        }
      }
    }

    public static IEnumerable<RegisterRequest> ValidUnconfirmedAccountSeedData
    {
      get
      {
        if (_validUnconfirmedAccountSeedData is null)
          _validUnconfirmedAccountSeedData = SeedDataGenerator.GetRegisterRequests(3, true);

        return _validUnconfirmedAccountSeedData;
      }
    }

    public static IEnumerable<object[]> NonExistingAccountLoginRequests
      => SeedDataGenerator.GetLoginRequests(3, true).Select(request => new object[] { request });
  }
}

