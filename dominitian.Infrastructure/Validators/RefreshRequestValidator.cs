using dominitian_ui.Models.Requests.Login;
using FluentValidation;

namespace dominitian.Infrastructure.Validators
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
