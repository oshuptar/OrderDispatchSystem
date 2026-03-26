using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.ToTable("OutboxEvents");
        builder.HasKey(outboxEvent => outboxEvent.Id);
        builder.Property(outboxEvent => outboxEvent.Id).ValueGeneratedOnAdd();
        builder.Property(outboxEvent => outboxEvent.Topic).IsRequired();
        builder.Property(outboxEvent => outboxEvent.EventType).IsRequired();
        builder.Property(outboxEvent => outboxEvent.Key).IsRequired();
        builder.Property(outboxEvent => outboxEvent.Payload).IsRequired();
    }
}