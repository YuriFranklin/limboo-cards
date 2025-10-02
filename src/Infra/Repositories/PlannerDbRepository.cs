namespace LimbooCards.Infra.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class PlannerDbRepository(AppDbContext _context, IMapper _mapper) : IPlannerRepository
    {
        private readonly IMapper mapper = _mapper;
        private readonly AppDbContext context = _context;
        private readonly DbSet<Planner> db = _context.Planners;

        public async Task AddPlannerAsync(Planner planner)
        {
            try
            {
                await db.AddAsync(planner);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task DeletePlannerAsync(string plannerId)
        {
            await context.Planners
            .Where(p => p.Id == plannerId)
            .ExecuteDeleteAsync();
        }

        public async Task<List<Planner>> GetAllPlannersAsync()
        {
            return await db
            .Include(p => p.Buckets)
            .Include(p => p.PinRules)
            .ToListAsync();
        }

        public async Task<Planner?> GetPlannerByIdAsync(string id)
        {
            return await db
            .Include(p => p.Buckets)
            .Include(p => p.PinRules)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Planner planner)
        {
            db.Update(planner);
            await context.SaveChangesAsync();
        }
    }
}
