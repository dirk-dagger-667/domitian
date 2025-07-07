using domitian.Tests.Integration.Helpers;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace domitian.Tests.Integration
{
  public class TestServer : WebApplicationFactory<Program>
  {
    public TestServer() { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      IConfiguration? appConfig = null;
      string? environment = null;

      builder.ConfigureAppConfiguration((context, config) =>
      {
        var apiDir = typeof(Program).Assembly.Location;
        appConfig = context.Configuration;
        environment = context.HostingEnvironment.EnvironmentName;
      });

      builder.ConfigureTestServices(services =>
      {
        services.RemoveAll(typeof(DbContextOptions<DomitianIDDbContext>));
        services.AddDbContext<DomitianIDDbContext>(options =>
          options.UseSqlServer(
            appConfig?.GetConnectionString(AppConstants.DomitianIntegrationTestsConnectionString),
            options => options.MigrationsAssembly(AppConstants.dbContextAssembly)));
        services.AddScoped<TestDbSeeder>();
        services.AddSingleton<ApiUrlPathBuilder>();
      });
    }
  }
}
