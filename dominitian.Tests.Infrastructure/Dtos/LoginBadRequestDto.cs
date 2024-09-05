using dominitian_ui.Models.Results;

namespace dominitian.Tests.Infrastructure.Dtos
{
    public record LoginBadRequestDto : LoginFailureDto
    {
        public required Error Error { get; init; }
    }
}
