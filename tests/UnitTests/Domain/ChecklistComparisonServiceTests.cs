namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistComparisonServiceTests
    {
        private readonly ChecklistComparisonService _service;

        public ChecklistComparisonServiceTests()
        {
            _service = new ChecklistComparisonService();
        }

        [Fact]
        public void GetCompletedChecklistItems_ShouldReturnCompletedItems()
        {
            var checklistItem1 = new ChecklistItem(
                id: "1",
                title: "Item1",
                isChecked: false,
                orderHint: "1",
                updatedAt: DateTime.UtcNow,
                updatedBy: Guid.NewGuid()
            );

            var card = new Card(
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.NewGuid(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.NewGuid(),
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item1", ContentStatus.Missing)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = _service.GetCompletedChecklistItems(card, subject);

            Assert.Single(result);
            Assert.Equal(card.Id, result[0].CardId);
            Assert.Equal(subject.Id, result[0].SubjectId);
            Assert.Equal(checklistItem1.Id, result[0].ChecklistItemId);
        }

        [Fact]
        public void GetCompletedChecklistItems_ShouldReturnEmpty_WhenNoMatches()
        {
            var checklistItem1 = new ChecklistItem(
                id: "1",
                title: "Item1",
                isChecked: false,
                orderHint: "1",
                updatedAt: DateTime.UtcNow,
                updatedBy: Guid.NewGuid()
            );

            var card = new Card(
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.NewGuid(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.NewGuid(),
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item2", ContentStatus.OK)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = _service.GetCompletedChecklistItems(card, subject);

            Assert.Empty(result);
        }

        [Fact]
        public void GetNotFoundChecklistItems_ShouldReturnNotFoundItems()
        {
            var checklistItem1 = new ChecklistItem(
                id: "1",
                title: "Item1",
                isChecked: false,
                orderHint: "1",
                updatedAt: DateTime.UtcNow,
                updatedBy: Guid.NewGuid()
            );

            var card = new Card(
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.NewGuid(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.NewGuid(),
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item2", ContentStatus.Missing),
                    new Content("Content2", "Item1", ContentStatus.OK)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = _service.GetNotFoundChecklistItems(card, subject);

            Assert.Single(result);
            Assert.Equal("Item2", result[0].ChecklistItemTitle);
            Assert.Equal(card.Id, result[0].CardId);
            Assert.Equal(subject.Id, result[0].SubjectId);
        }

        [Fact]
        public void GetNotFoundChecklistItems_ShouldReturnEmpty_WhenAllItemsExist()
        {
            var checklistItem1 = new ChecklistItem(
                id: "1",
                title: "Item1",
                isChecked: false,
                orderHint: "1",
                updatedAt: DateTime.UtcNow,
                updatedBy: Guid.NewGuid()
            );

            var card = new Card(
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.NewGuid(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.NewGuid(),
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item1", ContentStatus.Missing)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = _service.GetNotFoundChecklistItems(card, subject);

            Assert.Empty(result);
        }
    }
}
