using dominitian.Tests.Infrastructure.Dtos;
using dominitian.Tests.Infrastructure.Extensions;
using dominitian_ui.Models.Requests.Login;
using dominitian_ui.Models.Results;
using domition_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace dominitian.Tests.Infrastructure.DataSources.UserAdmin.Services
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
                    SignInRes = null,
                    PassSignInthrowsEx = false
                }};

                yield return new object[] { new LoginFailureDto()
                {
                    User = A.Dummy<DominitianIDUser>(),
                    CheckPassRes = false,
                    SignInRes= null,
                    PassSignInthrowsEx = false
                }};

                yield return new object[] { new LoginFailureDto()
                {
                    User = A.Dummy<DominitianIDUser>(),
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
                    User = A.Dummy<DominitianIDUser>(),
                    CheckPassRes = true,
                    SignInRes = null,
                    PassSignInthrowsEx = true,
                    Error = Error.Exception
                }};

                yield return new object[] { new LoginBadRequestDto()
                {
                    User = A.Dummy<DominitianIDUser>(),
                    CheckPassRes = true,
                    SignInRes = SignInResult.Failed,
                    PassSignInthrowsEx = false,
                    Error = LoginErrors.FailedAttempt
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
                        User = new DominitianIDUser()
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
                        User = new DominitianIDUser()
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

        public static IEnumerable<object[]> RevokeAccessFailureParams
        {
            get
            {
                yield return new object[]
                {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = null,
                        User = A.Dummy<DominitianIDUser>(),
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
                        User = A.Dummy<DominitianIDUser>(),
                        UpdRes = null
                    }
                };

                yield return new object[]
                {
                    new RevokeAccessUnauthorizedDto()
                    {
                        UserName = A.Dummy<string>(),
                        User = A.Dummy<DominitianIDUser>(),
                        UpdRes = IdentityResult.Failed()
                    }
                };
            }
        }
    }
}
