using domitian.Models.Requests.Users;
using domitian_api.Configuraion;
using domitian_api.Validators;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace domitian.Tests.Fixtures.UserAdmin.Users
{
  public class PutEndpointsFixture : UserEndpointsBase
  {
    public PutEndpointsFixture()
      : base()
    {
      SingleLogger = A.Fake<ILogger<ValidationFilter<UpdateUserRequest>>>();
      BulkLogger = A.Fake<ILogger<ValidationFilter<IEnumerable<UpdateUserRequest>>>>();

      ValidationFilterSingleUnderTest = new ValidationFilter<UpdateUserRequest>(
        SingleLogger,
        new UpdateUserRequestValidator());

      ValidationFilterBulkUnderTest = new ValidationFilter<IEnumerable<UpdateUserRequest>>(
        BulkLogger,
        new UpdateUserRequestCollectionValidator(new UpdateUserRequestValidator()));
    }

    public ValidationFilter<UpdateUserRequest> ValidationFilterSingleUnderTest { get; set; }
    public ValidationFilter<IEnumerable<UpdateUserRequest>> ValidationFilterBulkUnderTest { get; set; }

    public ILogger<ValidationFilter<UpdateUserRequest>> SingleLogger { get; set; }
    public ILogger<ValidationFilter<IEnumerable<UpdateUserRequest>>> BulkLogger { get; set; }
  }
}
