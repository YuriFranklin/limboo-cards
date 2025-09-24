namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistItemNotFoundedTests
    {
        [Fact]
        public void Constructor_ValidData_ShouldCreateInstance()
        {
            var checklistItemId = "1";
            var checklistItemTitle = "Checklist Item X";
            var cardId = Guid.CreateVersion7().ToString();
            var subjectId = Guid.CreateVersion7();

            var evt = new ChecklistItemNotFounded(checklistItemId, checklistItemTitle, cardId, subjectId);

            Assert.Equal(checklistItemTitle, evt.ChecklistItemTitle);
            Assert.Equal(cardId, evt.CardId);
            Assert.Equal(subjectId, evt.SubjectId);
        }

        [Fact]
        public void Constructor_EmptyTitle_ShouldThrowArgumentException()
        {
            var cardId = Guid.CreateVersion7().ToString();
            var subjectId = Guid.CreateVersion7();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1", "", cardId, subjectId));
        }

        [Fact]
        public void Constructor_EmptyCardId_ShouldThrowArgumentException()
        {
            var subjectId = Guid.CreateVersion7();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1,", "Valid Title", string.Empty, subjectId));
        }

        [Fact]
        public void Constructor_EmptySubjectId_ShouldThrowArgumentException()
        {
            var cardId = Guid.CreateVersion7().ToString();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1,", "Valid Title", cardId, Guid.Empty));
        }
    }
}
