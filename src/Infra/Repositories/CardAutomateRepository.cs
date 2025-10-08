namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.DTOs;
    using LimbooCards.Infra.Settings;
    using Microsoft.Extensions.Options;

    public class CardAutomateRepository(HttpClient httpClient, IMapper mapper, IOptions<CardSettings> options) : ICardRepository
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IMapper mapper = mapper;
        private readonly CardSettings settings = options.Value;

        public async Task AddCardAsync(Card card)
        {
            Console.WriteLine(card.ToString());
        }

        public Task DeleteCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            var result = await httpClient.GetFromJsonAsync<IEnumerable<CardAutomateDto>>(settings.GetAllUrl);
            return result?.Select(dto => mapper.Map<Card>(dto)) ?? [];
        }

        public async Task<Card?> GetCardByIdAsync(string cardId)
        {
            var url = $"{settings.GetByIdUrl}&card-id={cardId}";
            var result = await httpClient.GetFromJsonAsync<CardAutomateDto>(url);
            return result is null ? null : mapper.Map<Card>(result);
        }

        public async Task UpdateCardAsync(Card card)
        {
            if (card is null || string.IsNullOrWhiteSpace(card.Id))
            {
                throw new ArgumentException("Card or Card.Id cannot be null for update.");
            }

            var cardDto = mapper.Map<CardAutomateDto>(card);
            var url = $"{settings.UpdateUrl}&card-id={card.Id}";
            var response = await httpClient.PutAsJsonAsync(url, cardDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
