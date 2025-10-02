using domitian.Models.Dtos.UserAdmin;
using domitian_api.Configuraion;
using domitian_api.Validators;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace domitian.Tests.Fixtures.UserAdmin.Users
{
  public class GetEndpointsFixture : UserEndpointsBase
  {
    public GetEndpointsFixture()
      : base()
    {
      Logger = A.Fake<ILogger<ValidationFilter<PaganationParameters>>>();

      ValidationFilterUnderTest = new ValidationFilter<PaganationParameters>(
        Logger,
        new PaganationParamsValidator());
    }

    public ValidationFilter<PaganationParameters> ValidationFilterUnderTest { get; set; }
    public ILogger<ValidationFilter<PaganationParameters>> Logger { get; set; }
  }
}
