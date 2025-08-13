using domitian.Models.Requests.Login;
using FluentValidation;

namespace domitian_api.Validators
{
    public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
    {
        public RefreshRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
            RuleFor(x => x.AccessToken).NotNull().NotEmpty();
        }
    }
}
