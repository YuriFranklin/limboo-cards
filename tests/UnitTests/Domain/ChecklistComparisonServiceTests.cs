namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistComparisonServiceTests
    {
        [Fact]
        public void GetCompletedChecklistItems_ShouldReturnCompletedItems()
        {
            var checklistItem1 = new ChecklistItem(
                id: "1",
                title: "Item1",
                isChecked: false,
                orderHint: "1",
                updatedAt: DateTime.UtcNow,
                updatedBy: Guid.CreateVersion7().ToString()
            );

            var card = new Card(
                id: Guid.CreateVersion7().ToString(),
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.CreateVersion7().ToString(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.CreateVersion7(),
                modelId: null,
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item1", ContentStatus.Missing)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = ChecklistComparisonService.GetCompletedChecklistItems(card, subject);

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
                updatedBy: Guid.CreateVersion7().ToString()
            );

            var card = new Card(
                id: Guid.CreateVersion7().ToString(),
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.CreateVersion7().ToString(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.CreateVersion7(),
                modelId: null,
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item2", ContentStatus.OK)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = ChecklistComparisonService.GetCompletedChecklistItems(card, subject);

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
                updatedBy: Guid.CreateVersion7().ToString()
            );

            var card = new Card(
                id: Guid.CreateVersion7().ToString(),
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.CreateVersion7().ToString(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.CreateVersion7(),
                modelId: null,
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

            var result = ChecklistComparisonService.GetNotFoundChecklistItems(card, subject);

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
                updatedBy: Guid.CreateVersion7().ToString()
            );

            var card = new Card(
                id: Guid.CreateVersion7().ToString(),
                title: "Card1",
                description: "Description",
                hasDescription: true,
                createdBy: Guid.CreateVersion7().ToString(),
                checklist: new List<ChecklistItem> { checklistItem1 }
            );

            var subject = new Subject(
                id: Guid.CreateVersion7(),
                modelId: null,
                name: "Subject1",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                contents: new List<Content>
                {
                    new Content("Content1", "Item1", ContentStatus.Missing)
                },
                oferts: new List<Ofert> { new Ofert("PRJ", "M1") }
            );

            var result = ChecklistComparisonService.GetNotFoundChecklistItems(card, subject);

            Assert.Empty(result);
        }
    }
}
