namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class UserApplicationService(IUserRepository userRepository, IMapper mapper)
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly IMapper mapper = mapper;

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = mapper.Map<User>(dto);
            await userRepository.AddUserAsync(user);
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await userRepository.GetUserByIdAsync(dto.Id)
                ?? throw new ArgumentException($"User with Id {dto.Id} not found.");

            var updatedUser = new User(user.Id, dto.FullName);

            await userRepository.UpdateUserAsync(updatedUser);

            return mapper.Map<UserDto>(updatedUser);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await userRepository.DeleteUserAsync(userId);
        }
    }
}
