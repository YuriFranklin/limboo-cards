namespace LimbooCards.Presentation.GraphQL.Mutations
{
    using LimbooCards.Presentation.GraphQL.Contracts;
    using LimbooCards.Application.Services;
    using AutoMapper;
    using LimbooCards.Presentation.GraphQL.Models;
    using LimbooCards.Application.DTOs;

    public class CardMutations(
        CardApplicationService cardService,
        IMapper mapper
    )
    {
        private readonly CardApplicationService _cardService = cardService;
        private readonly IMapper _mapper = mapper;
        public async Task<NormalizeCardsOutput> NormalizeCardsAsync(List<string> cardIds)
        {
            var dto = await _cardService.NormalizeCardsAsync(cardIds);

            return new NormalizeCardsOutput
            {
                Success = [.. (dto.Success ?? Enumerable.Empty<CardDto>()).Select(c => _mapper.Map<CardModel>(c))],
                Failed = [.. dto.Failed ?? Enumerable.Empty<string>()]
            };
        }
    }
}