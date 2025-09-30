namespace LimbooCards.Presentation.GraphQL.Queries
{
    using AutoMapper;
    using LimbooCards.Application.Services;
    using LimbooCards.Presentation.GraphQL.Models;
    public class PlannerQueries(
        PlannerApplicationService plannerService,
        IMapper mapper
    )
    {
        private readonly PlannerApplicationService _plannerService = plannerService;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PlannerModel>> GetPlannersAsync()
        {
            var plannersDto = await _plannerService.GetAllPlannersAsync();
            return [.. plannersDto.Select(p => _mapper.Map<PlannerModel>(p))];
        }

        public async Task<PlannerModel?> GetPlannerAsync(string id)
        {
            var plannerDto = await _plannerService.GetPlannerByIdAsync(id);
            if (plannerDto == null)
            {
                return null;
            }

            return _mapper.Map<PlannerModel>(plannerDto);
        }
    }
}