using dominitian_ui.Models.Results;
using domition_api.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record LoginFailureDto
    {
        public DominitianIDUser? User { get; init; }
        public bool CheckPassRes { get; init; }
        public SignInResult? SignInRes { get; init; }
        public bool PassSignInthrowsEx { get; init; }
    }
}
