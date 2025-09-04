namespace LimbooCards.Presentation.GraphQL.Queries
{
    using LimbooCards.Application.Services;
    using AutoMapper;
    using LimbooCards.Presentation.GraphQL.Models;

    public class CardQueries(
        CardApplicationService cardService,
        IMapper mapper
    )
    {
        private readonly CardApplicationService _cardService = cardService;
        private readonly IMapper _mapper = mapper;

        public async Task<ChecklistResultModel> VerifyChecklistForCardsAsync(List<Guid> ids)
        {
            var flatResult = new ChecklistResultModel();

            foreach (var id in ids)
            {
                var dto = await _cardService.VerifyChecklistItemsAsync(id);
                if (dto != null)
                {
                    flatResult.Completed.AddRange(
                        (dto.Completed ?? new()).Select(c => _mapper.Map<ChecklistItemCompletedModel>(c))
                    );

                    flatResult.NotFound.AddRange(
                        (dto.NotFound ?? new()).Select(n => _mapper.Map<ChecklistItemNotFoundedModel>(n))
                    );
                }
            }

            return flatResult;
        }
    }
}