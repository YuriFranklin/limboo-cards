namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface ICardRepository
    {
        public Task<Card?> GetCardByIdAsync(Guid cardId);
        public Task<IEnumerable<Card>> GetAllCardsAsync();
        public Task AddCardAsync(Card card);
        public Task UpdateCardAsync(Card card);
        public Task DeleteCardAsync(Guid cardId);
    }
}