using domitian.Models.Results;

namespace domitian.Tests.Infrastructure.DataSources.UserAdmin.Controllers
{
    public class RegisterControllerTestData
    {
        public static IEnumerable<object[]> ConfirmRegistrationAsyncBadRequestErrors
        {
            get
            {
                yield return new object[] { Error.Exception };
                yield return new object[] { RegisterErrors.RegisterInvalidEmail };
            }
        }

        public static IEnumerable<object[]> RegisterAsyncBadRequestErrors
        {
            get
            {
                yield return new object[] { Error.Exception };
                yield return new object[] { RegisterErrors.RegisterUserExists(null) };
                yield return new object[] { RegisterErrors.RegisterUserAddToRoleFails(null) };
                yield return new object[] { RegisterErrors.RegisterCreateAccount(string.Empty) };
            }
        }
    }
}
