namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistItemCompletedTests
    {
        [Fact]
        public void ChecklistItemCompleted_ShouldInitializeProperties()
        {
            // Arrange
            var checklistItemId = "1";
            var completedBy = Guid.CreateVersion7();
            var completedAt = DateTime.UtcNow;
            var cardId = Guid.CreateVersion7();
            var subjectId = Guid.CreateVersion7();

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
                    Guid.CreateVersion7(),
                    DateTime.UtcNow,
                    Guid.CreateVersion7(),
                    Guid.CreateVersion7());
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
                    Guid.CreateVersion7(),
                    Guid.CreateVersion7());
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
                    Guid.CreateVersion7(),
                    default,
                    Guid.CreateVersion7(),
                    Guid.CreateVersion7());
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
                    Guid.CreateVersion7(),
                    DateTime.UtcNow,
                    Guid.Empty,
                    Guid.CreateVersion7());
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
                    Guid.CreateVersion7(),
                    DateTime.UtcNow,
                    Guid.CreateVersion7(),
                    Guid.Empty);
            });

            Assert.Contains("SubjectId must be set.", ex.Message);
        }
    }
}
