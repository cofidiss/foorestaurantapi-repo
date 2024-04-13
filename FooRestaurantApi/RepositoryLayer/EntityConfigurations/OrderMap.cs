using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations
{
    public class OrderMap : BaseMap<OrderEntity>, IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {

            builder.ToTable("order");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.Address).HasColumnName("address");
            builder.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            builder.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            builder.Property(e => e.Note).HasColumnName("note");
            builder.Property(e => e.StatusId).HasColumnName("status_id");
            base.Configure(builder);

        }


    }
}
