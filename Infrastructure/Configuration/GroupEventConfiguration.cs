using Domain.Models.Event;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal class GroupEventConfiguration : BaseConfiguration<GroupEvent>
{
    public override void Configure(EntityTypeBuilder<GroupEvent> builder)
    {
        base.Configure(builder);

        builder.HasMany(ge => ge.Events)
            .WithOne(e => e.GroupEvent)
            .HasForeignKey(e => e.GroupEventId);
    }
}
