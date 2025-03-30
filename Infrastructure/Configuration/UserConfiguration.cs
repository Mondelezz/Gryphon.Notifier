using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasMany<IdentityUserRole<long>>()
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

        builder.HasMany<IdentityUserClaim<long>>()
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

        builder.HasMany<IdentityUserLogin<long>>()
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

        builder.HasMany<IdentityUserToken<long>>()
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();


        builder.HasMany<IdentityUserRole<long>>()
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

        builder.HasMany<IdentityRoleClaim<long>>()
                .WithOne()
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
    }
}
