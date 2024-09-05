using dominitian_ui.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace dominitian_api.Helpers
{
    public class ReturnResultsHelper(IHttpContextAccessor _httpContextAccessor)
        : IReturnResultsHelper
    {
        public IActionResult ResultTypeToActionResult<T>(Result<T> result)
        {
            if (result.Type == ResultTypes.CreatedAt) 
            {
                return new CreatedResult(GetUri(), result.Data);
            }

            if (result.Type == ResultTypes.Ok)
            {
                return new OkObjectResult(result.Data);
            }

            return ResultTypeToActionResult(result);
        }

        public IActionResult ResultTypeToActionResult(Result result)
        {
            switch (result.Type)
            {
                case ResultTypes.Ok:
                    return new OkResult();
                case ResultTypes.BadRequest:
                    return new BadRequestObjectResult(result.Error?.Message);
                case ResultTypes.CreatedAt:
                    return new CreatedResult(GetUri(), null);
                case ResultTypes.NotFound:
                    return new NotFoundObjectResult(result.Error?.Message);
                case ResultTypes.Unauthorized:
                    return new UnauthorizedResult();
                default:
                    return new BadRequestObjectResult(result.Error?.Message);
            }
        }

        private string GetUri()
        {
            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            var uri = $"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.Path}{httpRequest?.QueryString}";

            return uri;
        }
    }
}
