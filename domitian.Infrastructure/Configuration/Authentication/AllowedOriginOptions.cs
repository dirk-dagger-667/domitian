namespace domitian.Infrastructure.Configuration.Authentication
{
    public class AllowedOriginOptions
    {
        public const string SectionName = "AllowedOrigin";

        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
