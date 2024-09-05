namespace dominitian_ui.Models.Responses.Login
{
    public record LoginResponse
    {
        public required string? Email { get; init; }

        public required string? BearerToken { get; init; }

        public required string? RefreshToken { get; init; }
    }
}
