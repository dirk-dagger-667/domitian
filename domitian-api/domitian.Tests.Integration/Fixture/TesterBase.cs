using domitian.Tests.Integration.DataSources;
using domitian.Tests.Integration.Helpers;
using domitian_api.Data.Identity;

namespace domitian.Tests.Integration.Fixture
{
  [CollectionDefinition("Integration tests")]
  public class IntegrationTestsCollectionDefenition : ICollectionFixture<TestServer> { }

  [Collection("Integration tests")]
  public abstract class TesterBase : IAsyncLifetime
  {
    private readonly TestServer _server;
    private AsyncServiceScope _scope;

    public IServiceProvider Services { get; }
    public HttpClient Client { get; set; }
    public IConfiguration Configuration { get; set; }
    public ApiUrlPathBuilder ApiUrlPathBuilder { get; set; }

    public TesterBase(TestServer testServer)
    {
      _server = testServer;
      _scope = _server.Services.CreateAsyncScope();
      Services = _scope.ServiceProvider;

      Client = _server.CreateClient();
      Configuration = Services.GetRequiredService<IConfiguration>();
      ApiUrlPathBuilder = Services.GetRequiredService<ApiUrlPathBuilder>();
    }

    public async Task InitializeAsync()
    {
      var context = Services.GetRequiredService<DomitianIDDbContext>();

      await context.Database.EnsureDeletedAsync();
      await context.Database.EnsureCreatedAsync();

      await SeedDatabaseAsync(context);
    }

    public async Task DisposeAsync()
    {
      var context = Services.GetRequiredService<DomitianIDDbContext>();

      await context.Database.EnsureDeletedAsync();

      await _scope.DisposeAsync();
    }

    private async Task SeedDatabaseAsync(DomitianIDDbContext? context)
    {
      var dbSeeder = Services.GetRequiredService<TestDbSeeder>();

      foreach (var request in LoginControllerTestData.ValidAccountSeedData)
      {
        await dbSeeder.BuildUserAsync(request, true);
      }

      foreach (var request in LoginControllerTestData.ValidUnconfirmedAccountSeedData)
      {
        await dbSeeder.BuildUserAsync(request);
      }

      await context?.SaveChangesAsync();
    }
  }
}
