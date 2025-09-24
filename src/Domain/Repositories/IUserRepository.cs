namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface IUserRepository
    {
        public Task<User?> GetUserByIdAsync(string userId);
        public Task<IEnumerable<User>> GetUsersByNameAsync(string fullName);
        public Task DeleteUserAsync(string userId);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task AddUserAsync(User user);
        public Task UpdateUserAsync(User user);
    }
}