namespace domitian.Models.Responses.Registration
{
    public record RegistrationResponse
    {
        public string? Code { get; init; }
        public string? User { get; init; }
    }
}
