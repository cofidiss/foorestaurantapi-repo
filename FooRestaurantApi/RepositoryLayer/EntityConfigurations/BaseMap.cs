using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations
{
    public class BaseMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {


            builder.Property(e => e.CreationDate).HasColumnName("creation_date");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.LastUpdateDate).HasColumnName("last_update_date");
            builder.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

        }


    }
}
