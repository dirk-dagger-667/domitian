using domitian.Models.Dtos.UserAdmin;
using FluentValidation;

namespace domitian_api.Validators
{
  public class PaganationParamsValidator : AbstractValidator<PaganationParameters>
  {
    public PaganationParamsValidator()
    {
      RuleLevelCascadeMode = CascadeMode.Stop;

      RuleFor(x => x.PageNumber).GreaterThan(0);
      RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
    }
  }
}
