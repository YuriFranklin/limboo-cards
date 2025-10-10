namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface ICardRepository
    {
        public Task<Card?> GetCardByIdAsync(string cardId);
        public Task<IEnumerable<Card>> GetAllCardsAsync();
        public Task<Card> AddCardAsync(Card card);
        public Task UpdateCardAsync(Card card);
        public Task DeleteCardAsync(string cardId);
    }
}