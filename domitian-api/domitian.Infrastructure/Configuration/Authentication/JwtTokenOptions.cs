namespace domitian.Infrastructure.Configuration.Authentication
{
    public class JwtTokenOptions
    {
        public const string SectionName = "JWT";

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public string? SigningKey { get; set; }
    }
}
