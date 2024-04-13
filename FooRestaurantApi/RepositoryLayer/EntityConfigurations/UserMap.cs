using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations.Postgre
{
    public class UserMap : BaseMap<UserEntity>, IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {

            builder.ToTable("user");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserName).HasColumnName("user_name");
            builder.Property(e => e.Password).HasColumnName("password");
            builder.Property(e => e.Address).HasColumnName("address");
            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            builder.Property(e => e.Phone).HasColumnName("phone");

            base.Configure(builder);

        }
    }
}
