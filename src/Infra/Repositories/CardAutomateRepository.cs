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

        public Task DeleteCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            var url = Environment.GetEnvironmentVariable("CARD_GETALL_URL")
                ?? throw new InvalidOperationException("CARD_GETALL_URL not set.");

            var result = await httpClient.GetFromJsonAsync<IEnumerable<CardAutomateDto>>(url);

            return result?.Select(dto => mapper.Map<Card>(dto))
                   ?? [];
        }

        public async Task<Card?> GetCardByIdAsync(string cardId)
        {
            var url = Environment.GetEnvironmentVariable("CARD_GETBYID_URL")
            ?? throw new InvalidOperationException("CARD_GETBYID_URL not set.");

            var result = await this.httpClient.GetFromJsonAsync<CardAutomateDto>($"{url}&card-id={cardId}");

            if (result == null) { return null; }

            return mapper.Map<Card>(result);
        }

        public Task UpdateCardAsync(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
