namespace LimbooCards.Presentation.GraphQL.Mutations
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Application.Services;
    using LimbooCards.Presentation.GraphQL.Contracts;
    using LimbooCards.Presentation.GraphQL.Models;

    public class PlannerMutations(
        PlannerApplicationService plannerService,
        IMapper mapper
    )
    {
        private readonly PlannerApplicationService _plannerService = plannerService;
        private readonly IMapper _mapper = mapper;

        public async Task<PlannerModel> CreatePlannerAsync(CreatePlannerInput input)
        {
            var dtoIn = _mapper.Map<CreatePlannerDto>(input);
            var dtoOut = await _plannerService.CreatePlannerAsync(dtoIn);
            return _mapper.Map<PlannerModel>(dtoOut);
        }

        public async Task<bool> DeletePlannerAsync(string id)
        {
            await _plannerService.DeletePlannerAsync(id);
            return true;
        }
    }
}