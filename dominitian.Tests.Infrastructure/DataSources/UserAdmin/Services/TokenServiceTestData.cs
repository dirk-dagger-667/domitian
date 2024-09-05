using domition_api.Data.Identity;

namespace dominitian.Tests.Infrastructure.DataSources.UserAdmin.Services
{
    public class TokenServiceTestData
    {
        public static IEnumerable<object[]> GenerateTokenTestDataNonNull
        {
            get
            {
                yield return new object[] { new DominitianIDUser() { UserName = "fake", Email = "fake@fake.com" }};

                yield return new object[] { new DominitianIDUser() { UserName = "asd123!@#", Email = "asd123!#@asd.com.com" }};

                yield return new object[] { new DominitianIDUser() { UserName = string.Empty, Email = string.Empty }};
            }
        }

        public static IEnumerable<object[]> GenerateTokenTestDataNull
        {
            get
            {
                yield return new object[] { null, typeof(NullReferenceException) };
                yield return new object[] { new DominitianIDUser() { UserName = null, Email = null }, typeof(ArgumentNullException) };
            }
        }
    }
}
