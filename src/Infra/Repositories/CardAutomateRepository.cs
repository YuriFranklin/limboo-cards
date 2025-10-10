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

        public async Task<Card> AddCardAsync(Card card)
        {
            var url = $"{settings.CreateUrl}";
            var response = await httpClient.PostAsJsonAsync(url, card);
            var cardAutomateDtoResponse = await response.Content.ReadFromJsonAsync<CardAutomateDto>();
            return mapper.Map<Card>(cardAutomateDtoResponse);
        }

        public async Task DeleteCardAsync(string cardId)
        {
            var url = $"{settings.DeleteUrl}&card-id={cardId}";
            await httpClient.DeleteAsync(url);
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
            var url = $"{settings.UpdateUrl}&card-id={card.Id}";
            var response = await httpClient.PutAsJsonAsync(url, card);
            response.EnsureSuccessStatusCode();
        }
    }
}
