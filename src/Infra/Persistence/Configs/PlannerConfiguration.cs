namespace LimbooCards.Infra.Persistence.Configs
{
    using LimbooCards.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class PlannerConfiguration : IEntityTypeConfiguration<Planner>
    {
        public void Configure(EntityTypeBuilder<Planner> builder)
        {
            builder.ToTable("Planners");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .IsRequired();

            builder.HasMany(p => p.Buckets)
                   .WithOne()
                   .HasForeignKey("PlannerId")
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasMany(p => p.PinRules)
                   .WithOne()
                   .HasForeignKey("PlannerId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.Buckets)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(p => p.PinRules!)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
