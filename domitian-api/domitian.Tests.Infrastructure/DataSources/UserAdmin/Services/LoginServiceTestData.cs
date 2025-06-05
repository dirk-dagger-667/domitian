using domitian.Tests.Infrastructure.Dtos;
using domitian.Models.Requests.Login;
using domitian.Models.Results;
using domitian_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace domitian.Tests.Infrastructure.DataSources.UserAdmin.Services
{
  public class LoginServiceTestData
  {
    public static IEnumerable<object[]> LoginUnauthorizedParams
    {
      get
      {
        yield return new object[] { new LoginFailureDto()
                {
                    User = null, // Only this value is important 
                    CheckPassRes = false,
                    SignInRes = SignInResult.Success,
                    PassSignInthrowsEx = false
                }};

        yield return new object[] { new LoginFailureDto()
                {
                    User = A.Dummy<DomitianIDUser>(),
                    CheckPassRes = false,
                    SignInRes= null,
                    PassSignInthrowsEx = false
                }};

        yield return new object[] { new LoginFailureDto()
                {
                    User = A.Dummy<DomitianIDUser>(),
                    CheckPassRes = true,
                    SignInRes = SignInResult.LockedOut,
                    PassSignInthrowsEx = false,
                }};
      }
    }

    public static IEnumerable<object[]> LoginBadRequestParams
    {
      get
      {
        yield return new object[] { new LoginBadRequestDto()
                {
                    User = A.Dummy<DomitianIDUser>(),
                    CheckPassRes = true,
                    SignInRes = SignInResult.Failed,
                    PassSignInthrowsEx = false,
                    Error = LoginErrors.FailedAttempt
                }};
      }
    }

    public static IEnumerable<object[]> LoginExceptionParams
    {
      get
      {
        yield return new object[] { new LoginFailureDto()
                {
                    User = A.Dummy<DomitianIDUser>(),
                    CheckPassRes = true,
                    SignInRes = null,
                    PassSignInthrowsEx = false
                }};
      }
    }

    public static IEnumerable<object[]> RefreshAccessUnauthorizedParams
    {
      get
      {
        var fake = "fake";

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = null,
                        ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity()),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = null
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = null,
                        ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity()),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = null
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = new DomitianIDUser(),
                        ClaimsPrincipal= new ClaimsPrincipal(new ClaimsIdentity( new Claim[]
                        {
                            new Claim(ClaimTypes.Name, fake)
                        })),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = string.Empty
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = new DomitianIDUser()
                        {
                            RefreshToken = fake,
                            RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5),
                            Email = fake
                        },
                        ClaimsPrincipal= new ClaimsPrincipal(new ClaimsIdentity( new Claim[]
                        {
                            new Claim(ClaimTypes.Name, fake)
                        })),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = $"{fake}1"
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = new DomitianIDUser()
                        {
                            RefreshToken = fake,
                            RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(-5),
                            Email = fake
                        },
                        ClaimsPrincipal= new ClaimsPrincipal(new ClaimsIdentity( new Claim[]
                        {
                            new Claim(ClaimTypes.Name, fake)
                        })),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = fake
                        }
                    }
        };
      }
    }

    public static IEnumerable<object[]> RefreshAccessExceptionParams
    {
      get
      {
        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = null,
                        ClaimsPrincipal = null,
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = null
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = null,
                        ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                          new Claim(ClaimTypes.Name, "fake")
                        })),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = null
                        }
                    }
        };

        yield return new object[]
        {
                    new RefreshAccessBaseDto()
                    {
                        User = new DomitianIDUser
                        {
                          RefreshToken = null,
                          RefreshTokenExpiry = DateTime.UtcNow.AddDays(1)
                        },
                        ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                          new Claim(ClaimTypes.Name, "fake")
                        })),
                        RefReq = new RefreshRequest()
                        {
                            AccessToken = string.Empty,
                            RefreshToken = null
                        }
                    }
        };
      }
    }

    public static IEnumerable<object[]> RevokeAccessUnauthorizedParams
    {
      get
      {
        yield return new object[]
        {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = null,
                        User = A.Dummy<DomitianIDUser>(),
                        UpdRes = IdentityResult.Success
                    }
        };

        yield return new object[]
        {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = A.Dummy<string>(),
                        User = null,
                        UpdRes = IdentityResult.Success
                    }
        };

        yield return new object[]
        {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = A.Dummy<string>(),
                        User = A.Dummy<DomitianIDUser>(),
                        UpdRes = null
                    }
        };

        yield return new object[]
        {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = A.Dummy<string>(),
                        User = A.Dummy<DomitianIDUser>(),
                        UpdRes = IdentityResult.Failed()
                    }
        };
      }
    }
  }
}
