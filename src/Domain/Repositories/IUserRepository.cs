namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(Guid userId);
        public Task DeleteUserAsync(Guid userId);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task AddUserAsync(User user);
        public Task UpdateUserAsync(User user);
    }
}