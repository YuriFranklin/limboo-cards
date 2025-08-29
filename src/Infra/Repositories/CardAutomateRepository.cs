namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.DTOs;

    public class CardAutomateRepository(HttpClient httpClient, IMapper mapper) : ICardRepository
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IMapper mapper = mapper;

        public Task AddCardAsync(Card card)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCardAsync(Guid cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            var result = await this.httpClient.GetFromJsonAsync<IEnumerable<CardAutomateDto>>("/cards");

            if (result == null || !result.Any()) { return []; }

            return mapper.Map<IEnumerable<Card>>(result);
        }

        public async Task<Card?> GetCardByIdAsync(Guid cardId)
        {
            var result = await this.httpClient.GetFromJsonAsync<CardAutomateDto>($"/cards/{cardId}");

            if (result == null) { return null; }

            return mapper.Map<Card>(result);
        }

        public Task UpdateCardAsync(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
