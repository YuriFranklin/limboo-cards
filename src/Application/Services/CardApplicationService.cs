namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class CardApplicationService(ICardRepository cardRepository, IMapper mapper)
    {
        private readonly ICardRepository cardRepository = cardRepository;
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
    }
}