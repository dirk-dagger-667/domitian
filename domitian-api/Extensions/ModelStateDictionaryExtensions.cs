using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace domitian_api.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static string GetErrorsAsString(this ModelStateDictionary dict)
        {
            return dict.Values
                .SelectMany(val => val.Errors.Select(er => er.ErrorMessage))
                .Aggregate(string.Empty, (current, next) => $"{current}{Environment.NewLine}{next}");
        }
    }
}
