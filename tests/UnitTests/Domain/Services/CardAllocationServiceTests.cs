namespace LimbooCards.UnitTests.Domain.Services
{
    public class CardAllocationServiceTests
    {

        private static Card CreateValidCard(
            string? id = "card1",
            string? bucketId = null,
            string? planId = "planner1",
            bool withChecklist = true)
        {
            return new Card(
                title: "Sample Card",
                hasDescription: false,
                createdBy: "tester",
                planId: planId!,
                id: id,
                bucketId: bucketId,
                checklist: withChecklist
                    ? new List<ChecklistItem> { new("chk1", "Item 1", false, "a", DateTime.UtcNow, "tester") }
                    : new List<ChecklistItem>()
            );
        }

        private static Subject CreateValidSubject(bool withMissingContent = true)
        {
            return new Subject(
                id: Guid.NewGuid(),
                modelId: null,
                name: "Math",
                semester: "2025-1",
                status: SubjectStatus.Complete,
                oferts: new List<Ofert> { new("PRJ", "M1") },
                contents: withMissingContent
                    ? new List<Content> { new("Topic B", "item1", ContentStatus.Missing, 5) }
                    : new List<Content>()
            );
        }

        private static Planner CreateValidPlanner(List<PlannerBucket>? customBuckets = null)
        {
            return new Planner(
                id: "planner1",
                name: "Main Planner",
                buckets: customBuckets ?? new List<PlannerBucket>
                {
                    new(Guid.NewGuid().ToString(), "Default Bucket", isDefault: true),
                    new(Guid.NewGuid().ToString(), "Topic A Bucket", isDefault: false),
                    new(Guid.NewGuid().ToString(), "End Bucket", isEnd: true),
                    new(Guid.NewGuid().ToString(), "History Bucket", isHistory: true)
                }
            );
        }

        [Fact]
        public void AllocateCardToBucket_ShouldKeepCardInEndBucket_WhenNoMissingContent()
        {
            // Arrange
            var planner = CreateValidPlanner();
            var endBucket = planner.Buckets.First(b => b.IsEnd);
            var card = CreateValidCard(bucketId: endBucket.Id, planId: planner.Id);
            var subject = CreateValidSubject(withMissingContent: false);

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().NotBeNull();
            result!.PlannerId.Should().Be(planner.Id);
            result.BucketId.Should().Be(endBucket.Id);
        }

        [Fact]
        public void AllocateCardToBucket_ShouldAllocate_ToDefaultBucket_WhenNoMatchFound()
        {
            // Arrange
            var card = CreateValidCard();
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();
            var defaultBucket = planner.Buckets.First(b => b.IsDefault);

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().NotBeNull();
            result!.CardId.Should().Be(card.Id);
            result.PlannerId.Should().Be(planner.Id);
            result.BucketId.Should().Be(defaultBucket.Id);
        }

        [Fact]
        public void AllocateCardToBucket_ShouldBeNull_WhenSubjectHasNoMissingContent_AndCardIsNotInEndBucket()
        {
            // Arrange
            var card = CreateValidCard();
            var subject = CreateValidSubject(withMissingContent: false);
            var planner = CreateValidPlanner();

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void AllocateCardToBucket_ShouldBeNull_WhenCardHasNoId()
        {
            // Arrange
            var card = CreateValidCard(id: null);
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void AllocateCardToBucket_ShouldBeNull_WhenCardHasNoChecklist()
        {
            // Arrange
            var card = CreateValidCard(withChecklist: false);
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().BeNull();
        }
    }
}