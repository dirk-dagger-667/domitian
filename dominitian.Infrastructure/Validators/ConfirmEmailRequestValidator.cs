using dominitian_ui.Models.Requests.Registration;
using FluentValidation;

namespace dominitian_ui.Infrastructure.Validators
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
