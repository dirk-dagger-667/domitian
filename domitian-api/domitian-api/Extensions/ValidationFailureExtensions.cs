using FluentValidation.Results;

namespace domitian_api.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static string ConcatErrors(this IEnumerable<ValidationFailure> valFails)
            => string.Join(Environment.NewLine, valFails.Select(failure => failure.ErrorMessage));
    }
}
