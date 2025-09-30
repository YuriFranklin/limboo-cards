namespace LimbooCards.Infra.Persistence.Configs
{
    using LimbooCards.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class PlannerBucketConfiguration : IEntityTypeConfiguration<PlannerBucket>
    {
        public void Configure(EntityTypeBuilder<PlannerBucket> builder)
        {
            builder.ToTable("PlannerBuckets");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                   .IsRequired();

            builder.Property(b => b.Name)
                   .IsRequired();

            builder.Property(b => b.IsDefault)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne<Planner>()
                   .WithMany(p => p.Buckets)
                   .HasForeignKey("PlannerId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
