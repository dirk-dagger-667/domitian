using domitian.Models.Results.Errors;

namespace domitian.Tests.Infrastructure.Dtos
{
    public record LoginBadRequestDto : LoginFailureDto
    {
        public required Error Error { get; init; }
    }
}
