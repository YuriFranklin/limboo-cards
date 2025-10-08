namespace LimbooCards.UnitTests.Domain.Events
{
    public class ChecklistItemCompletedTests
    {
        private readonly string _validTitle = "Item 1";
        private readonly string _validId = "chk1";
        private readonly string _validUser = "user1";
        private readonly DateTime _validDate = DateTime.UtcNow;
        private readonly string _validCardId = "card1";
        private readonly Guid _validSubjectId = Guid.NewGuid();

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenDataIsValid()
        {
            // Act
            var evt = new ChecklistItemCompleted(_validTitle, _validId, _validUser, _validDate, _validCardId, _validSubjectId);

            // Assert
            evt.ChecklistItemTitle.Should().Be(_validTitle);
            evt.ChecklistItemId.Should().Be(_validId);
            evt.CompletedBy.Should().Be(_validUser);
            evt.CompletedAt.Should().Be(_validDate);
            evt.CardId.Should().Be(_validCardId);
            evt.SubjectId.Should().Be(_validSubjectId);
        }


        [Fact]
        public void Constructor_ShouldThrow_WhenValueTypeArgumentsAreInvalid()
        {
            // Arrange
            Action actForDate = () => new ChecklistItemCompleted(_validTitle, _validId, _validUser, default, _validCardId, _validSubjectId);
            Action actForSubjectId = () => new ChecklistItemCompleted(_validTitle, _validId, _validUser, _validDate, _validCardId, Guid.Empty);

            // Act & Assert
            actForDate.Should().Throw<ArgumentException>().WithMessage("CompletedAt must be set.*");
            actForSubjectId.Should().Throw<ArgumentException>().WithMessage("SubjectId must be set.*");
        }
    }
}