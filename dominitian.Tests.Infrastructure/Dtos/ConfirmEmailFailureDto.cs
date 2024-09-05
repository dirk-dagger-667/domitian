using dominitian_ui.Models.Requests.Registration;
using dominitian_ui.Models.Results;
using domition_api.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record ConfirmEmailFailureDto
    {
        public required ConfirmEmailRequest ConfEmailReq { get; init; }
        public DominitianIDUser? User { get; init; }
        public IdentityResult? IdRes { get; init; }
        public required Error Error { get; set; }
        public required Action<Result, Error> Assertion { get; init; }
    }
}
