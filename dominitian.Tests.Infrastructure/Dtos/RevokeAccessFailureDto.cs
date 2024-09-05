using domition_api.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record RevokeAccessUnauthorizedDto
    {
        public string? UserName { get; init; }
        public DominitianIDUser? User { get; init; }
        public IdentityResult? UpdRes { get; init; }
    }
}
