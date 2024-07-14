using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Entity.TaskTakip;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooRestaurantApi.RepositoryLayer.EntityConfigurations
{
    public class TaskMap : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {

            builder.ToTable("task");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.PercentCompletion).HasColumnName("percent_completion");
            builder.Property(e => e.WorkedHour).HasColumnName("worked_hour");
            builder.Property(e => e.CategoryId).HasColumnName("category_id");
            builder.Property(e => e.EstimatedWorkHour).HasColumnName("estimated_work_hour");

            builder.Property(e => e.WeekId).HasColumnName("week_id");
            builder.Property(e => e.PercentCompletion).HasColumnName("percent_completion");

            builder.Property(e => e.Description).HasColumnName("description");
;

 

        }


    }
}
