using dominitian_ui.Models.Results;
using domition_api.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record RegisterBadReqDto
    {
        public DominitianIDUser? User { get; init; }
        public IdentityResult? IdRes { get; init; }
        public bool SupportsEmail { get; init; }
        public IdentityResult? AddToRoleRes { get; init; }
        public required Error Error { get; init; }
    }
}
