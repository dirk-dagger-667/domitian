namespace dominitian.Infrastructure.Configuration
{
    public class AllowedOriginOptions
    {
        public const string SectionName = "AllowedOrigin";

        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
