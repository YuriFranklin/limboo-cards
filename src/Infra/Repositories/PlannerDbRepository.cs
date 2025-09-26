namespace LimbooCards.Infra.Repositories
{
    using System.Collections.Generic;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.Persistence;

    public class PlannerDbRepository(AppDbContext _context, IMapper _mapper) : IPlannerRepository
    {
        private readonly IMapper mapper = _mapper;
        private readonly Microsoft.EntityFrameworkCore.DbSet<Planner> db = _context.Planners;

        public Task AddPlannerAsync(Planner planner)
        {
            throw new NotImplementedException();
        }

        public Task<List<Planner>> GetAllPlannersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Planner?> GetPlannerByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Planner planner)
        {
            throw new NotImplementedException();
        }
    }
}
