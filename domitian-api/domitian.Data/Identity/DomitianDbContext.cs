using domitian.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace domitian_api.Data.Identity
{
  public class DomitianIDDbContext : IdentityDbContext<DomitianIDUser>
  {
    public DomitianIDDbContext(DbContextOptions<DomitianIDDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      base.OnConfiguring(optionsBuilder
        .ConfigureConection(configuration)
        .SeedRoles(DbContextOptionsBuilderExtensions.BuildRoleList()));
    }
  }
}
