using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal sealed class TopicConfiguration : BaseConfiguration<Topic>
{
    public override void Configure(EntityTypeBuilder<Topic> builder)
    {
        base.Configure(builder);

        builder.HasMany(ge => ge.Events)
            .WithOne(e => e.Topic)
            .HasForeignKey(e => e.TopicId);
    }
}
