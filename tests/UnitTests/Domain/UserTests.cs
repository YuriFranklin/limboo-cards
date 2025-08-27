#nullable disable
namespace LimbooCards.UnitTests.Domain
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldInitializeProperties_WhenValidParametersProvided()
        {
            var id = Guid.NewGuid();
            var fullName = "Test User";
            var email = "test@user.com";

            var user = new User(id, fullName, email);

            Assert.Equal(id, user.Id);
            Assert.Equal(fullName, user.FullName);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void User_ShouldThrowException_WhenIdIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.Empty, "Test User", "test@user.com"));
            Assert.Contains("Id must be set.", exception.Message);
        }

        [Fact]
        public void User_ShouldThrowException_WhenEmailIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.NewGuid(), "Test User", null));
            Assert.Contains("Email cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_ShouldThrowException_WhenUsernameIsInvalid(string invalidFullName)
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.NewGuid(), invalidFullName, "test@user.com"));
            Assert.Equal("FullName cannot be empty. (Parameter 'FullName')", exception.Message);
        }
    }
}
