#nullable disable
namespace LimbooCards.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldInitializeProperties_WhenValidParametersProvided()
        {
            var id = Guid.NewGuid();
            var username = "testuser";

            var user = new User(id, username);

            Assert.Equal(id, user.Id);
            Assert.Equal(username, user.Username);
        }

        [Fact]
        public void User_ShouldThrowException_WhenIdIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.Empty, "testuser"));
            Assert.Contains("Id must be set.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_ShouldThrowException_WhenUsernameIsInvalid(string invalidUsername)
        {
            var id = Guid.NewGuid();
            var exception = Assert.Throws<ArgumentException>(() => new User(id, invalidUsername));
            Assert.Equal("Username cannot be empty. (Parameter 'Username')", exception.Message);
        }
    }
}
