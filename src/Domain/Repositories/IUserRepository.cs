namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(Guid userId);
    }
}