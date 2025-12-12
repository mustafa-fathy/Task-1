using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presisitence.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(k => k.OrderId);

            builder.Property(p => p.Deleted);
            builder.HasQueryFilter(p => !p.Deleted);

            builder.Property(p => p.CreatedAt)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()");


        }
    }
}
