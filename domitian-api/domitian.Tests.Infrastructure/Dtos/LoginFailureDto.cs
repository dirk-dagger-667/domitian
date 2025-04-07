using domitian_api.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace domitian.Tests.Infrastructure.Dtos
{
    public record LoginFailureDto
    {
        public DomitianIDUser? User { get; init; }
        public bool CheckPassRes { get; init; }
        public SignInResult? SignInRes { get; init; }
        public bool PassSignInthrowsEx { get; init; }
    }
}
