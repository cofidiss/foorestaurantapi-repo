using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations
{
    public class FoodCategoryMap : BaseMap<FoodCategoryEntity>, IEntityTypeConfiguration<FoodCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<FoodCategoryEntity> builder)
        {

            builder.ToTable("food_category");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.OrderInDay).HasColumnName("order_in_day");

            builder.Property(e => e.DayId).HasColumnName("day_id");

            base.Configure(builder);

        }


    }
}
