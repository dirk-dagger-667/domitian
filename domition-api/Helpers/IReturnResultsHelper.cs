using dominitian_ui.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace dominitian_api.Helpers
{
    public interface IReturnResultsHelper
    {
        IActionResult ResultTypeToActionResult(Result result);
        IActionResult ResultTypeToActionResult<T>(Result<T> result);
    }
}
