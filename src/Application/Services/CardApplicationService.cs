namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Domain.Services;

    public class CardApplicationService
    (
        ICardRepository cardRepository,
        ISubjectRepository subjectRepository,
        IMapper mapper
    )
    {
        private readonly ICardRepository cardRepository = cardRepository;
        private readonly ISubjectRepository subjectRepository = subjectRepository;
        private readonly IMapper mapper = mapper;

        public async Task<CardDto> CreateCardAsync(CreateCardDto dto)
        {
            var card = mapper.Map<Card>(dto);

            await this.cardRepository.AddCardAsync(card);

            return mapper.Map<CardDto>(card);
        }

        public async Task<CardDto?> GetCardByIdAsync(Guid cardId)
        {
            var card = await this.cardRepository.GetCardByIdAsync(cardId);
            if (card == null) return null;

            return mapper.Map<CardDto>(card);
        }

        public async Task<IEnumerable<CardDto>> GetAllCardsAsync()
        {
            var cards = await this.cardRepository.GetAllCardsAsync();
            return mapper.Map<IEnumerable<CardDto>>(cards);
        }

        public async Task UpdateCardAsync(UpdateCardDto dto)
        {
            var card = mapper.Map<Card>(dto);

            await this.cardRepository.UpdateCardAsync(card);
        }

        public async Task DeleteCardAsync(Guid cardId)
        {
            await this.cardRepository.DeleteCardAsync(cardId);
        }

        public async Task<ChecklistResultDto?> VerifyChecklistItemsAsync(Guid cardId)
        {
            var card = await this.cardRepository.GetCardByIdAsync(cardId);
            if (card == null)
                return null;

            Subject? subject;

            if (!card.SubjectId.HasValue)
            {
                var subjects = await this.subjectRepository.GetAllSubjectsAsync();
                subject = SubjectMatcherService.MatchSubjectForCard(card, subjects);
            }
            else
            {
                subject = await this.subjectRepository.GetSubjectByIdAsync(card.SubjectId!.Value);
            }

            if (subject == null)
                return null;

            var completed = ChecklistComparisonService.GetCompletedChecklistItems(card, subject);
            var notFound = ChecklistComparisonService.GetNotFoundChecklistItems(card, subject);

            return new ChecklistResultDto
            {
                Completed = completed,
                NotFound = notFound
            };
        }
    }
}