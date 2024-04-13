using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations
{
    public class FoodMap : BaseMap<FoodEntity>, IEntityTypeConfiguration<FoodEntity>
    {
        public void Configure(EntityTypeBuilder<FoodEntity> builder)
        {

            builder.ToTable("food");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.OrderInCategory).HasColumnName("order_in_category");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            builder.Property(e => e.Price).HasColumnName("price");
            builder.Property(e => e.Image).HasColumnName("image");
            builder.Property(e => e.CategoryId).HasColumnName("category_id");
            base.Configure(builder);

        }


    }
}
