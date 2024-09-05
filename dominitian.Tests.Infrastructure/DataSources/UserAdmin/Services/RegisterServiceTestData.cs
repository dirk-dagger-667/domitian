using dominitian.Tests.Infrastructure.Dtos;
using dominitian.Tests.Infrastructure.Extensions;
using dominitian_ui.Models.Requests.Registration;
using dominitian_ui.Models.Results;
using domition_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.DataSources.UserAdmin.Services
{
    public class RegisterServiceTestData
    {
        public static IEnumerable<object[]> ConfirmEmailFailureParams
        {
            get 
            {
                yield return new object[] 
                {
                    new ConfirmEmailFailureDto() 
                    {
                        ConfEmailReq = new ConfirmEmailRequest()
                        {
                            UserId = string.Empty,
                            Code = string.Empty
                        },
                        User = null,
                        IdRes = null,
                        Error = RegisterErrors.RegisterUserNull,
                        Assertion = ResultAssertions.NotFound
                    }
                };

                yield return new object[] 
                {
                    new ConfirmEmailFailureDto()
                    {
                        ConfEmailReq = new ConfirmEmailRequest()
                        {
                            UserId = string.Empty,
                            Code = string.Empty
                        },
                        User = A.Dummy<DominitianIDUser>(),
                        IdRes = IdentityResult.Failed(A.Dummy<IdentityError>()),
                        Error = RegisterErrors.RegisterInvalidEmail,
                        Assertion = ResultAssertions.IsBadRequest
                    }
                };
            }
        }

        public static IEnumerable<object[]> RegisterAsyncBadRequestParams
        {
            get
            {
                yield return new object[] { new RegisterBadReqDto() 
                { 
                    User = null,
                    IdRes = IdentityResult.Failed(A.Dummy<IdentityError>()),
                    SupportsEmail = true,
                    AddToRoleRes = null,
                    Error = RegisterErrors.RegisterCreateAccount(null)
                }};

                yield return new object[] { new RegisterBadReqDto()
                {
                    User = A.Dummy<DominitianIDUser>(),
                    IdRes = null,
                    SupportsEmail = false,
                    AddToRoleRes = null,
                    Error = RegisterErrors.RegisterUserExists
                }};

                yield return new object[] { new RegisterBadReqDto()
                {
                    User = null,
                    IdRes = IdentityResult.Success,
                    AddToRoleRes = IdentityResult.Failed(A.Dummy<IdentityError>()),
                    SupportsEmail = true,
                    Error = RegisterErrors.RegisterUserAddToRoleFails
                }};
            }
        }
    }
}
