using dominitian_ui.Models.Requests.Login;
using domition_api.Data.Identity;
using System.Security.Claims;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record RefreshAccessBaseDto()
    {
        public RefreshAccessBaseDto(string fakeName, int additionalMins)
            : this()
        {
            User = new DominitianIDUser()
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
        public DominitianIDUser? User { get; init; }
        public ClaimsPrincipal? ClaimsPrincipal { get; init; }
    }
}
