using domitian.Models.Requests.Login;
using domitian_api.Data.Identity;
using System.Security.Claims;

namespace domitian.Tests.Infrastructure.Dtos
{
    public record RefreshAccessBaseDto()
    {
        public RefreshAccessBaseDto(string fakeName, int additionalMins)
            : this()
        {
            User = new DomitianIDUser()
            {
                RefreshToken = fakeName,
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(additionalMins),
                Email = fakeName
            };

            RefReq = new RefreshRequest()
            {
                AccessToken = fakeName,
                RefreshToken = fakeName
            };

            ClaimsPrincipal = new ClaimsPrincipal(new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, fakeName)
                })));
        }

        public required RefreshRequest RefReq { get; init; }
        public DomitianIDUser? User { get; init; }
        public ClaimsPrincipal? ClaimsPrincipal { get; init; }
    }
}
