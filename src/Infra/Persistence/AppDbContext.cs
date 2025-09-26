namespace LimbooCards.Infra.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using LimbooCards.Domain.Entities;
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Planner> Planners => Set<Planner>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}