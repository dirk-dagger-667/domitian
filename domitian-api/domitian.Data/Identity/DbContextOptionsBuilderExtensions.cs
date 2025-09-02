using domitian_api.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Const = domitian.Data.Constants;


namespace domitian.Data.Identity
{
  public static class DbContextOptionsBuilderExtensions
  {
    public static DbContextOptionsBuilder ConfigureConection(this DbContextOptionsBuilder builder, IConfiguration configuration)
      => builder.UseSqlServer(configuration.GetConnectionString(AppConstants.DomitianConnectionString),
        options => options.MigrationsAssembly(AppConstants.DBContextAssembly));

    public static DbContextOptionsBuilder SeedRoles(this DbContextOptionsBuilder builder, IEnumerable<IdentityRole> roles)
    => builder.UseSeeding((context, _) =>
    {
      if (context.Set<IdentityRole>().Any())
      {
        context.Set<IdentityRole>().AddRange(roles);
        context.SaveChanges();
      }
    })
      .UseAsyncSeeding(async (context, _, _) =>
      {
        if (!await context.Set<IdentityRole>().AnyAsync())
        {
          await context.Set<IdentityRole>().AddRangeAsync(roles);
          await context.SaveChangesAsync();
        }
      });

    public static IEnumerable<IdentityRole> BuildRoleList()
      => new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name = Const.UserRoles.Admin,
                    NormalizedName = Const.UserRoles.Admin.ToUpperInvariant(),
                },
                new IdentityRole()
                {
                    Name = Const.UserRoles.FreeUser,
                    NormalizedName = Const.UserRoles.FreeUser.ToUpperInvariant(),
                },
                new IdentityRole()
                {
                    Name = Const.UserRoles.Tier1User,
                    NormalizedName = Const.UserRoles.Tier1User.ToUpperInvariant(),
                },
                new IdentityRole()
                {
                    Name = Const.UserRoles.Tier2User,
                    NormalizedName = Const.UserRoles.Tier2User.ToUpperInvariant(),
                },
                new IdentityRole()
                {
                    Name = Const.UserRoles.Tier3User,
                    NormalizedName = Const.UserRoles.Tier3User.ToUpperInvariant(),
                }
            };
  }
}
