namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class PlannerApplicationService(
        IPlannerRepository plannerRepository,
        IMapper mapper
    )
    {
        private readonly IMapper mapper = mapper;
        private readonly IPlannerRepository plannerRepository = plannerRepository;

        public async Task<PlannerDto> CreatePlannerAsync(CreatePlannerDto dto)
        {
            var planner = mapper.Map<Planner>(dto);

            await this.plannerRepository.AddPlannerAsync(planner);

            return mapper.Map<PlannerDto>(planner);
        }

        public async Task<PlannerDto?> GetPlannerByIdAsync(string plannerId)
        {
            var planner = await this.plannerRepository.GetPlannerByIdAsync(plannerId);
            if (planner == null) return null;

            return mapper.Map<PlannerDto>(planner);
        }

        public async Task<IEnumerable<PlannerDto>> GetAllPlannersAsync()
        {
            var planners = await this.plannerRepository.GetAllPlannersAsync();
            return mapper.Map<IEnumerable<PlannerDto>>(planners);
        }

        public async Task DeletePlannerAsync(string plannerId)
        {
            await this.plannerRepository.DeletePlannerAsync(plannerId);
        }
    }
}