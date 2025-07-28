using domitian.Business.Contracts;
using domitian.Infrastructure.Configuration.Authentication;
using domitian_api.Data.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace domitian.Business.Services
{
    public class TokenService(IOptions<JwtTokenOptions> _jwtOptions) : ITokenService
    {
        private readonly SymmetricSecurityKey _key = _jwtOptions.Value.SigningKey != null
            ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SigningKey))
            : throw new ArgumentException("Jwt's signing key is null.");

        public string GenerateJwt(DomitianIDUser user)
        {
            var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(0.5),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenObj = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenObj);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var validationOptions = new TokenValidationParameters
            {
                ValidIssuer = _jwtOptions.Value.Issuer,
                ValidAudience = _jwtOptions.Value.Audience,
                IssuerSigningKey = _key,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validationOptions, out _);
        }
    }
}
