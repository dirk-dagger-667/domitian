using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace domitian.Data.Identity
{
  public class DomitianIDDbContext : IdentityDbContext<DomitianIDUser>
  {
    public DomitianIDDbContext(DbContextOptions<DomitianIDDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(DomitianIDUser).Assembly);
      base.OnModelCreating(builder);
    }
  }
}
