namespace LimbooCards.UnitTests.Application
{
    public class UserApplicationServiceTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        private readonly UserApplicationService service;

        public UserApplicationServiceTests()
        {
            service = new UserApplicationService(userRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Create_And_Return_UserDto()
        {
            var dto = new CreateUserDto { FullName = "John Doe", Name = "John", Email = "user@test.com" };
            var user = new User(Guid.CreateVersion7().ToString(), dto.Name, dto.FullName, dto.Email);
            var userDto = new UserDto { Id = user.Id, FullName = user.FullName };

            mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);
            mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await service.CreateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.FullName, result.FullName);
            userRepositoryMock.Verify(r => r.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_Return_UserDto_When_Found()
        {
            var userId = Guid.CreateVersion7().ToString();
            var user = new User(userId, "Jane", "Jane Doe", "user@test.com");
            var userDto = new UserDto { Id = userId, FullName = user.FullName };

            userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await service.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(user.FullName, result.FullName);
        }

        [Fact]
        public async Task GetUsersByNameAsync_Should_Return_UserDtos_When_Found()
        {
            var fullName = "Jane";
            var users = new List<User>
            {
                new(Guid.CreateVersion7().ToString(), "Jane", "Jane Doe", "user1@test.com"),
                new(Guid.CreateVersion7().ToString(), "Alice","Alice", "user2@test.com"),
                new(Guid.CreateVersion7().ToString(), "Bob","Bob", "user3@test.com")
            };

            var expectedDtos = users
                .Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase))
                .Select(u => new UserDto { Id = u.Id, FullName = u.FullName })
                .ToList();

            userRepositoryMock
                .Setup(r => r.GetUsersByNameAsync(fullName))
                .ReturnsAsync([.. users.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase))]);

            mapperMock
                .Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns(expectedDtos);

            var result = await service.GetUsersByNameAsync(fullName);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, dto => Assert.Contains(fullName, dto.FullName, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_List_Of_UserDto()
        {
            var users = new List<User>
            {
                new User(Guid.CreateVersion7().ToString(), "Alice", "Alice", "user@test.com"),
                new User(Guid.CreateVersion7().ToString(), "Bob", "Bob", "user@test.com")
            };
            var userDtos = users.Select(u => new UserDto { Id = u.Id, FullName = u.FullName }).ToList();

            userRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);
            mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            var result = await service.GetAllUsersAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.FullName == "Alice");
            Assert.Contains(result, r => r.FullName == "Bob");
        }

        [Fact]
        public async Task UpdateUserAsync_Should_Update_And_Return_UserDto()
        {
            var dto = new UpdateUserDto { Id = Guid.CreateVersion7().ToString(), Name = "Updated", FullName = "Updated Name", Email = "user@test.com" };
            var existingUser = new User(dto.Id, "Old", "Old Name", "user@test.com");
            var updatedUser = new User(dto.Id, dto.Name, dto.FullName, "user@test.com");
            var updatedUserDto = new UserDto { Id = dto.Id, FullName = dto.FullName };

            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.Id)).ReturnsAsync(existingUser);
            mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(updatedUserDto);

            var result = await service.UpdateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.FullName, result.FullName);
            userRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<User>(u => u.Id == dto.Id && u.FullName == dto.FullName)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_Exception_When_User_NotFound()
        {
            var dto = new UpdateUserDto { Id = Guid.CreateVersion7().ToString(), FullName = "Name" };

            userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(dto.Id))
                .ThrowsAsync(new ArgumentException($"User with Id {dto.Id} not found."));

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => service.UpdateUserAsync(dto)
            );

            Assert.Equal($"User with Id {dto.Id} not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Call_Repository()
        {
            var userId = Guid.CreateVersion7().ToString();

            await service.DeleteUserAsync(userId);

            userRepositoryMock.Verify(r => r.DeleteUserAsync(userId), Times.Once);
        }
    }
}
