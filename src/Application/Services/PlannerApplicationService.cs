namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class PlannerApplicationService(
        IPlannerRepository plannerRepository,
        ICardRepository cardRepository,
        IMapper mapper
    )
    {
        private readonly IMapper mapper = mapper;
        private readonly IPlannerRepository plannerRepository = plannerRepository;
        private readonly ICardRepository cardRepository = cardRepository;


        public async Task<PlannerDto> CreatePlannerAsync(CreatePlannerDto dto)
        {
            var planner = mapper.Map<Planner>(dto);

            await plannerRepository.AddPlannerAsync(planner);

            return mapper.Map<PlannerDto>(planner);
        }

        public async Task<PlannerDto?> GetPlannerByIdAsync(string plannerId)
        {
            var planner = await plannerRepository.GetPlannerByIdAsync(plannerId);
            if (planner == null) return null;

            return mapper.Map<PlannerDto>(planner);
        }

        public async Task<IEnumerable<PlannerDto>> GetAllPlannersAsync()
        {
            var planners = await plannerRepository.GetAllPlannersAsync();
            return mapper.Map<IEnumerable<PlannerDto>>(planners);
        }

        public async Task DeletePlannerAsync(string plannerId)
        {
            await plannerRepository.DeletePlannerAsync(plannerId);
        }

        public async Task ArchiveFinishedCards(string plannerId)
        {
            var planner = await plannerRepository.GetPlannerByIdAsync(plannerId);
            if (planner == null) return;

            var cards = await cardRepository.GetAllCardsAsync();

            foreach (Card card in cards)
            {
                var bucket = planner.Buckets.FirstOrDefault(b => b.Id == card.BucketId);

                if (bucket == null || !bucket.IsHistory || card.Id == null) continue;

                if (card.Checklist != null && !card.Checklist.All(c => c.IsChecked)) continue;

                await cardRepository.DeleteCardAsync(card.Id);
            }
        }
    }
}