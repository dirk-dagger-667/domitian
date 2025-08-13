using domitian.Infrastructure.Configuration.Authentication;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace domitian_api.Extensions
{
  public static class IHostApplicationBuilderExtensions
  {
    public static IHostApplicationBuilder AddDomitianCors(this IHostApplicationBuilder builder)
    {
      builder.Services.AddCors(options =>
      {
        options.AddPolicy(name: AllowedOriginOptions.SectionName,
                          policy =>
                          {
                            policy
                              .WithOrigins(builder.Configuration["AllowedOrigin:Url"])
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                          });
      });

      return builder;
    }

    public static IHostApplicationBuilder AddDomitianDataAccessConfig<TContext, TUser>(this IHostApplicationBuilder builder)
      where TContext : DbContext
      where TUser : IdentityUser
    {
      builder.Services.AddDbContext<TContext>(options => options
        .UseSqlServer(builder
                    .Configuration
                    .GetConnectionString(AppConstants.DomitianConnectionString),
                    options => options.MigrationsAssembly(AppConstants.DBContextAssembly)));

      builder.Services.AddIdentity<TUser, IdentityRole>(options =>
      {
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
      })
          .AddEntityFrameworkStores<TContext>()
          .AddDefaultTokenProviders();

      return builder;
    }
  }
}
