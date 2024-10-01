using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Const = domitian.Data.Constants;

namespace domitian_api.Data.Identity
{
    public class DomitianIDDbContext : IdentityDbContext<DomitianIDUser>
    {
        public DomitianIDDbContext(DbContextOptions<DomitianIDDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var roles = new List<IdentityRole>()
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

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
