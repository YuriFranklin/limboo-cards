namespace LimbooCards.UnitTests
{
    public class ChecklistItemNotFoundedTests
    {
        [Fact]
        public void Constructor_ValidData_ShouldCreateInstance()
        {
            var checklistItemId = "1";
            var checklistItemTitle = "Checklist Item X";
            var cardId = Guid.NewGuid();
            var subjectId = Guid.NewGuid();

            var evt = new ChecklistItemNotFounded(checklistItemId, checklistItemTitle, cardId, subjectId);

            Assert.Equal(checklistItemTitle, evt.ChecklistItemTitle);
            Assert.Equal(cardId, evt.CardId);
            Assert.Equal(subjectId, evt.SubjectId);
        }

        [Fact]
        public void Constructor_EmptyTitle_ShouldThrowArgumentException()
        {
            var cardId = Guid.NewGuid();
            var subjectId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1", "", cardId, subjectId));
        }

        [Fact]
        public void Constructor_EmptyCardId_ShouldThrowArgumentException()
        {
            var subjectId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1,", "Valid Title", Guid.Empty, subjectId));
        }

        [Fact]
        public void Constructor_EmptySubjectId_ShouldThrowArgumentException()
        {
            var cardId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
                new ChecklistItemNotFounded("1,", "Valid Title", cardId, Guid.Empty));
        }
    }
}
