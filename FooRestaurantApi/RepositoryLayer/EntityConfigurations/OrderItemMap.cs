using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations.Postgre
{
    public class OrderItemMap : BaseMap<OrderItemEntity>, IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {

            builder.ToTable("order_item");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.OrderId).HasColumnName("order_id");
            builder.Property(e => e.Quantity).HasColumnName("quantity");
            builder.Property(e => e.FoodId).HasColumnName("food_id");
            base.Configure(builder);

        }
    }
}
