namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class UserAutomateRepository(HttpClient httpClient, IMapper mapper) : IUserRepository
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IMapper mapper = mapper;

        public Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersByNameAsync(string fullName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}