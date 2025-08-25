#nullable disable
namespace LimbooCards.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldInitializeProperties_WhenValidParametersProvided()
        {
            var id = Guid.NewGuid();
            var fullName = "Test User";

            var user = new User(id, fullName);

            Assert.Equal(id, user.Id);
            Assert.Equal(fullName, user.FullName);
        }

        [Fact]
        public void User_ShouldThrowException_WhenIdIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(Guid.Empty, "Test User"));
            Assert.Contains("Id must be set.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_ShouldThrowException_WhenUsernameIsInvalid(string invalidFullName)
        {
            var id = Guid.NewGuid();
            var exception = Assert.Throws<ArgumentException>(() => new User(id, invalidFullName));
            Assert.Equal("FullName cannot be empty. (Parameter 'FullName')", exception.Message);
        }
    }
}
