using domitian.Models.Results;

namespace domitian.Tests.Infrastructure.Dtos
{
    public record LoginBadRequestDto : LoginFailureDto
    {
        public required Error Error { get; init; }
    }
}
