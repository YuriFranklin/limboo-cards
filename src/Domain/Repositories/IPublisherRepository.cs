namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface IPublisherRepository
    {
        public Task<Publisher?> GetPublisherByIdAsync(Guid publisherId);
        public Task<Publisher?> GetPublisherByNameAsync(string name);
        public Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        public Task AddPublisherAsync(Publisher publisher);
        public Task UpdatePublisherAsync(Publisher publisher);
        public Task DeletePublisherAsync(Guid publisherId);
    }
}