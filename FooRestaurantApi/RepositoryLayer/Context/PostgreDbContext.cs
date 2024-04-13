using FooRestaurantApi.RepositoryLayer.EntityConfigurations;
using FooRestaurantApi.RepositoryLayer.EntityConfigurations.Postgre;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FooRestaurantApi.RepositoryLayer.Context
{
    public partial class PostgreDbContext : DbContext
    {
        public PostgreDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging().LogTo(message => Debug.WriteLine(message));


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new FoodMap());
            modelBuilder.ApplyConfiguration(new FoodCategoryMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
