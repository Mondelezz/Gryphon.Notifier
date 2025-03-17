using Domain.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal class ExternalIdentityConfiguration : BaseConfiguration<ExternalIdentity>
{
    public override void Configure(EntityTypeBuilder<ExternalIdentity> builder)
    {
        base.Configure(builder);

        builder.HasOne(ei => ei.User)
               .WithMany(ei => ei.ExternalIdentities)
               .HasForeignKey(ei => ei.UserId);
    }
}
