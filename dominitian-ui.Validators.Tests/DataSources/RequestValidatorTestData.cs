namespace dominitian_ui.Infrastructure.Tests.DataSources
{
    public class RequestValidatorTestData
    {
        public static IEnumerable<object[]> InvalidEmailData
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { string.Empty };
                yield return new object[] { "asdasd" };
                yield return new object[] { "asdasdasdasd.asdasd.asdasd" };
                yield return new object[] { "as75asd@as123sdasdasdas23sd" };
            }
        }

        public static IEnumerable<object[]> InvalidPswdData
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { string.Empty };
                yield return new object[] { "asdasd" };
                yield return new object[] { "!sdas@" };
                yield return new object[] { "asdasdasdasd" };
                yield return new object[] { "Asdasd!@#@@#@" };
                yield return new object[] { "asdasd  as@#@#" };
            }
        }

        public static IEnumerable<object[]> InvalidCnfrmPswd
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { string.Empty };
                yield return new object[] { "asdasd" };
                yield return new object[] { "!sdas@" };
                yield return new object[] { "asdasdasdasd" };
                yield return new object[] { "Asdasd!@#@@#@" };
                yield return new object[] { "asdasd  as@#@#" };
                yield return new object[] { "a@123ADS" };//Valid, but not the same as Password
            }
        }
    }
}
