namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistItemCompletedTests
    {
        [Fact]
        public void ChecklistItemCompleted_ShouldInitializeProperties()
        {
            // Arrange
            var checklistItemId = "1";
            var completedBy = Guid.NewGuid();
            var completedAt = DateTime.UtcNow;
            var cardId = Guid.NewGuid();
            var subjectId = Guid.NewGuid();

            // Act
            var evt = new ChecklistItemCompleted(
                checklistItemId,
                completedBy,
                completedAt,
                cardId,
                subjectId);

            // Assert
            Assert.Equal(checklistItemId, evt.ChecklistItemId);
            Assert.Equal(completedBy, evt.CompletedBy);
            Assert.Equal(completedAt, evt.CompletedAt);
            Assert.Equal(cardId, evt.CardId);
            Assert.Equal(subjectId, evt.SubjectId);
        }

        [Fact]
        public void ChecklistItemCompleted_ShouldThrow_WhenChecklistItemIdIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ChecklistItemCompleted(
                    null!,
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    Guid.NewGuid(),
                    Guid.NewGuid());
            });

            Assert.Contains("ChecklistItemId must be set.", ex.Message);
        }

        [Fact]
        public void ChecklistItemCompleted_ShouldThrow_WhenCompletedByIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ChecklistItemCompleted(
                    "1",
                    Guid.Empty,
                    DateTime.UtcNow,
                    Guid.NewGuid(),
                    Guid.NewGuid());
            });

            Assert.Contains("CompletedBy must be set.", ex.Message);
        }

        [Fact]
        public void ChecklistItemCompleted_ShouldThrow_WhenCompletedAtIsDefault()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ChecklistItemCompleted(
                    "1",
                    Guid.NewGuid(),
                    default,
                    Guid.NewGuid(),
                    Guid.NewGuid());
            });

            Assert.Contains("CompletedAt must be set.", ex.Message);
        }

        [Fact]
        public void ChecklistItemCompleted_ShouldThrow_WhenCardIdIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ChecklistItemCompleted(
                    "1",
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    Guid.Empty,
                    Guid.NewGuid());
            });

            Assert.Contains("CardId must be set.", ex.Message);
        }

        [Fact]
        public void ChecklistItemCompleted_ShouldThrow_WhenSubjectIdIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ChecklistItemCompleted(
                    "1",
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    Guid.NewGuid(),
                    Guid.Empty);
            });

            Assert.Contains("SubjectId must be set.", ex.Message);
        }
    }
}
