using FluentValidation.Results;

namespace dominitian_api.Extensions
{
    public static class ValidationFailureExtentions
    {
        //public static IDictionary<string, string[]> ToValFailDictionary(this IEnumerable<ValidationFailure> valFails)
        //{
        //    var res = valFails
        //    .Select<ValidationFailure, (string Key, string[] Value)>(err => new(err.PropertyName, [err.ErrorMessage]))
        //    .ToDictionary();

        //    return res;
        //}

        public static string ConcatErrors(this IEnumerable<ValidationFailure> valFails)
            => string.Join(Environment.NewLine, valFails.Select(failure => failure.ErrorMessage));
    }
}
