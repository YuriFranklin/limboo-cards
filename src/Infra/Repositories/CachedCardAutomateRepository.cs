namespace LimbooCards.Infra.Repositories
{
    using LimbooCards.Application.Ports;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CachedCardAuomateRepository(ICardRepository inner, IKeyValueStore cache) : ICardRepository
    {
        private readonly ICardRepository _inner = inner;
        private readonly IKeyValueStore _cache = cache;
        private readonly string _bucket = "cards";
        private readonly TimeSpan _ttl = TimeSpan.FromHours(1);

        public async Task AddCardAsync(Card card)
        {
            await _inner.AddCardAsync(card);
            _ = _cache.DeleteAsync(_bucket, "all");
        }

        public Task DeleteCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            var cachedList = await _cache.GetAsync<List<Card>>(_bucket, "all");
            if (cachedList != null)
                return cachedList;

            var cards = await _inner.GetAllCardsAsync();

            foreach (var card in cards)
                _ = _cache.PutAsync(_bucket, card.Id!, card, _ttl);

            _ = _cache.PutAsync(_bucket, "all", cards, _ttl);

            return cards;
        }

        public async Task<Card?> GetCardByIdAsync(string cardId)
        {
            var cached = await _cache.GetAsync<Card>(_bucket, cardId);
            if (cached != null) return cached;

            var card = await _inner.GetCardByIdAsync(cardId);
            if (card != null)
                _ = _cache.PutAsync(_bucket, cardId, card, _ttl);

            return card;
        }

        public Task UpdateCardAsync(Card card)
        {
            throw new NotImplementedException();
        }
    }
}