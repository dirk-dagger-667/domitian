using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domitian.Data.Identity
{
  internal class DomitianIDUserEntityTypeConfiguration : IEntityTypeConfiguration<DomitianIDUser>
  {
    public void Configure(EntityTypeBuilder<DomitianIDUser> builder)
    {
      builder
      .Property(user => user.Email)
      .IsRequired();

      builder.HasIndex(user => user.Email);
    }
  }
}
