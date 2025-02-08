using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal class EventConfiguration : BaseConfiguration<Event>
{
    public override void Configure(EntityTypeBuilder<Event> builder)
    {
        base.Configure(builder);

        builder.HasOne(e => e.Topic)
            .WithMany(ge => ge.Events)
            .HasForeignKey(e => e.TopicId);
    }
}
