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
        IPlannerRepository plannerRepository,
        IMapper mapper
    )
    {
        private readonly ICardRepository cardRepository = cardRepository;
        private readonly ISubjectRepository subjectRepository = subjectRepository;
        private readonly IPlannerRepository plannerRepository = plannerRepository;
        private readonly IMapper mapper = mapper;

        public async Task<CardDto> CreateCardAsync(CreateCardDto dto)
        {
            var card = mapper.Map<Card>(dto);

            await this.cardRepository.AddCardAsync(card);

            return mapper.Map<CardDto>(card);
        }
        public async Task<CardDto?> GetCardByIdAsync(string cardId)
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

        public async Task DeleteCardAsync(string cardId)
        {
            await this.cardRepository.DeleteCardAsync(cardId);
        }

        public async Task<ChecklistResultDto?> VerifyChecklistItemsAsync(string cardId)
        {
            var card = await this.cardRepository.GetCardByIdAsync(cardId);
            if (card == null)
                return null;

            Subject? subject;

            if (!card.SubjectId.HasValue)
            {
                var subjects = await this.subjectRepository.GetAllSubjectsAsync();
                subject = CardSubjectMatcherService.MatchSubjectForCard(card, subjects);
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

        public async Task<IEnumerable<ChecklistItemNormalizedDto>?> NormalizeCardChecklistAsync(string cardId)
        {
            var card = await cardRepository.GetCardByIdAsync(cardId);
            if (card == null)
            {
                return null;
            }

            var normalizedItems = CardChecklistNormalizeService.NormalizeChecklist(card);

            if (normalizedItems.Count == 0)
            {
                return null;
            }

            return mapper.Map<IEnumerable<ChecklistItemNormalizedDto>>(normalizedItems);
        }

        public async Task<NormalizeCardsResultDto> NormalizeCardsAsync(List<string> cardIds)
        {
            var allSubjects = await subjectRepository.GetAllSubjectsAsync();

            var results = await Task.WhenAll(cardIds.Select(async id =>
            {
                try
                {
                    var card = await cardRepository.GetCardByIdAsync(id);
                    if (card == null) return (Success: (CardDto?)null, Failed: id);

                    var subject = CardSubjectMatcherService.MatchSubjectForCard(card, allSubjects);
                    if (subject == null) return (Success: null, Failed: id);

                    var planner = await plannerRepository.GetPlannerByIdAsync(card.PlanId);
                    if (planner == null) return (Success: null, Failed: id);

                    var normalizedItems = CardChecklistNormalizeService.NormalizeChecklist(card);

                    List<ChecklistItem> newChecklist = [.. normalizedItems
                    .Select(n =>
                    {
                        var original = card?.Checklist?.FirstOrDefault(orig => orig.Id == n.ChecklistItemId);
                        return original?.With(title: n.NormalizedTitle);
                    })
                    .OfType<ChecklistItem>()];

                    var normalizedCard = card.With(checklist: newChecklist);

                    var notFoundedChecklistItems = ChecklistComparisonService.GetNotFoundChecklistItems(normalizedCard, subject);

                    var notFoundedItems = ChecklistComparisonService
                        .GetNotFoundChecklistItems(normalizedCard, subject)
                        .Select((nf, index) => new ChecklistItem(
                            id: nf.ChecklistItemId,
                            title: nf.ChecklistItemTitle,
                            isChecked: false,
                            orderHint: (index + 1).ToString(),
                            updatedAt: DateTime.UtcNow,
                            updatedBy: "system"
                        ));

                    var finalChecklist = newChecklist
                        .Concat(notFoundedItems)
                        .ToList();

                    var cardWithFinalChecklist = card.With(checklist: finalChecklist);

                    if (string.IsNullOrWhiteSpace(card.Id)) return (Success: null, Failed: id);

                    var appliedCategories = PinEvaluatorService
                        .EvaluateCardPins(subject, planner, card.Id)
                        ?.AppliedCategories ?? new Dictionary<string, bool>();

                    var cardPlannerAllocated = CardAllocationService
                        .AllocateCardToBucket(cardWithFinalChecklist, subject, planner);

                    var finalCard = card.With(
                        checklist: finalChecklist,
                        title: CardTitleNormalizeService.Normalize(subject),
                        subjectId: subject.Id,
                        appliedCategories: appliedCategories,
                        bucketId: cardPlannerAllocated?.BucketId,
                        planId: cardPlannerAllocated?.PlannerId
                        );

                    await cardRepository.UpdateCardAsync(finalCard);
                    return (Success: mapper.Map<CardDto>(finalCard), Failed: null!);
                }
                catch
                {
                    return (Success: null, Failed: id);
                }
            }));

            return new NormalizeCardsResultDto
            {
                Success = [.. results.Where(r => r.Success != null).Select(r => r.Success!)],
                Failed = [.. results.Where(r => r.Failed != null).Select(r => r.Failed!)]
            };
        }
    }
}