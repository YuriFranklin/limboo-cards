namespace LimbooCards.Infra.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using LimbooCards.Domain.Entities;
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Planner> Planners => Set<Planner>();
        public DbSet<PlannerBucket> PlannerBuckets => Set<PlannerBucket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}