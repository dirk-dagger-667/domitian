using domitian.Infrastructure.Configuration.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace domitian.Infrastructure.Configuration.OptionsSetup
{
    public class JwtBearerOptionsSetup(IOptions<JwtTokenOptions> _jwtOptions) : IConfigureOptions<JwtBearerOptions>
    {
        public void Configure(JwtBearerOptions bearerOptions)
        {
            var jwt = _jwtOptions.Value;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = jwt.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwt.SigningKey!)),
                ValidateLifetime = true
            };
        }
    }
}
