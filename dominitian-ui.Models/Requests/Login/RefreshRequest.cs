namespace dominitian_ui.Models.Requests.Login
{
    public record RefreshRequest
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
    }
}
