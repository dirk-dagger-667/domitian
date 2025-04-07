using domitian.Infrastructure.Validators;
using domitian.Models.Requests.Login;
using FluentValidation.TestHelper;

namespace domitian.Infrastructure.Tests.Validators
{
    public class RefreshRequestValidatorTester
    {
        private readonly RefreshRequestValidator _validator;
        private readonly RefreshRequest _validRequest;

        public RefreshRequestValidatorTester()
        {
            _validator = new RefreshRequestValidator();
            _validRequest = new RefreshRequest()
            {
                AccessToken = "fake",
                RefreshToken = "fake"
            };
        }

        [Theory]
        [InlineData("fake", null)]
        [InlineData(null, "fake")]
        [InlineData(null, null)]
        public void Should_have_error_when_either_token_is_Null(string? accTok, string? refTok)
        {
            var invalidReq = new RefreshRequest()
            {
                AccessToken = accTok,
                RefreshToken = refTok
            };

            var result = _validator.TestValidate(invalidReq);

            if (accTok is null)
            {
                result.ShouldHaveValidationErrorFor(x => x.AccessToken);
            }

            if (refTok is null)
            {
                result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
            }
        }
    }
}
