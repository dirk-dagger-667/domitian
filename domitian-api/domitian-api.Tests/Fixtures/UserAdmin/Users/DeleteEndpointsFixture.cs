using domitian_api.Configuraion;
using domitian_api.Validators;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace domitian.Tests.Fixtures.UserAdmin.Users
{
  public class DeleteEndpointsFixture : UserEndpointsBase
  {
    public DeleteEndpointsFixture()
    {
      Logger = A.Fake<ILogger<ValidationFilter<IEnumerable<string>>>>();
      IdsValidator = new ValidationFilter<IEnumerable<string>>(Logger,
        new StringCollectionValidator());
    }

    public ILogger<ValidationFilter<IEnumerable<string>>> Logger { get; set; }
    public ValidationFilter<IEnumerable<string>> IdsValidator { get; set; }
  }
}
