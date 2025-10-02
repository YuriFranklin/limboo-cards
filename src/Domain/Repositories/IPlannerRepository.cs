namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;

    public interface IPlannerRepository
    {
        public Task<Planner?> GetPlannerByIdAsync(string id);
        public Task AddPlannerAsync(Planner planner);
        public Task UpdateAsync(Planner planner);
        public Task<List<Planner>> GetAllPlannersAsync();
        public Task DeletePlannerAsync(string plannerId);
    }
}
