#nullable disable
namespace LimbooCards.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldInitializeProperties_WhenValidParametersProvided()
        {
            var id = Guid.CreateVersion7().ToString();
            var name = "Test";
            var fullName = "Test User";
            var email = "test@user.com";

            var user = new User(id, name, fullName, email);

            Assert.Equal(id, user.Id);
            Assert.Equal(name, user.Name);
            Assert.Equal(fullName, user.FullName);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void User_ShouldThrowException_WhenIdIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(string.Empty, "Test", "Test User", "test@user.com"));
            Assert.Contains("Id must be set.", exception.Message);
        }

        [Fact]
        public void User_ShouldThrowException_WhenEmailIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.CreateVersion7().ToString(), "Test", "Test User", null));
            Assert.Contains("Email cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_ShouldThrowException_WhenUsernameIsInvalid(string invalidName)
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.CreateVersion7().ToString(), invalidName, "Invalid Names", "test@user.com"));
            Assert.Equal("Name cannot be empty. (Parameter 'Name')", exception.Message);
        }
    }
}
