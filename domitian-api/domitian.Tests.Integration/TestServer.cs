using domitian.Tests.Integration.Helpers;
using domitian_api.Data.Identity;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace domitian.Tests.Integration
{
  public class TestServer : WebApplicationFactory<Program>, IAsyncLifetime
  {
    private readonly IDatabaseContainer _msSqlContainer = new MsSqlBuilder()
      .WithImage("mcr.microsoft.com/mssql/server:2019-CU27-ubuntu-20.04")
      .Build();

    public TestServer() { }

    public async Task InitializeAsync()
    {
      await _msSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      IConfiguration? appConfig = null;

      builder.ConfigureAppConfiguration((context, config) => appConfig = context.Configuration);

      builder.ConfigureTestServices(services =>
      {
        services.RemoveAll(typeof(DbContextOptions<DomitianIDDbContext>));
        //services.AddDbContext<DomitianIDDbContext>(options =>
        //  options.UseSqlServer(
        //    appConfig?.GetConnectionString(AppConstants.DomitianIntegrationTestsConnectionString),
        //    options => options.MigrationsAssembly(AppConstants.dbContextAssembly)));

        services.AddDbContext<DomitianIDDbContext>(options =>
        {
          var testContainerConfig = appConfig?
          .GetRequiredSection(SqlContainerTestConfig.SectionName)
          .Get<SqlContainerTestConfig>();

          var connString = _msSqlContainer
          .GetConnectionString()
          .Replace("Database=master", $"Database={testContainerConfig?.Database}");

          options.UseSqlServer(connString);
        });

        services.AddScoped<TestDbSeeder>();
        services.AddSingleton<ApiUrlPathBuilder>();
      });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
      await _msSqlContainer.StopAsync();
      await _msSqlContainer.DisposeAsync();
    }
  }
}
