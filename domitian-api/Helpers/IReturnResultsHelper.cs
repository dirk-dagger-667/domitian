using domitian.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace domitian_api.Helpers
{
    public interface IReturnResultsHelper
    {
        IActionResult ResultTypeToActionResultBase(Result result);
        IActionResult ResultTypeToActionResult<T>(Result<T> result);
    }
}
