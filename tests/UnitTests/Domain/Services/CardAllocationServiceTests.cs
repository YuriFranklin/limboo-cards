namespace LimbooCards.UnitTests.Domain.Services
{
    using LimbooCards.Domain.Entities;

    public class CardAllocationServiceTests
    {
        private static Card CreateValidCard(
            string? id = "card1",
            string? bucketId = null,
            string? planId = "planner1")
        {
            return new Card(
                title: "Sample Card",
                hasDescription: false,
                createdBy: "tester",
                planId: planId!,
                id: id,
                bucketId: bucketId,
                checklist: new List<ChecklistItem>
                {
                    new ChecklistItem("chk1","Item 1",false,"a",DateTime.UtcNow,"tester")
                }
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
                oferts: new List<Ofert> { new Ofert(project: "PRJ", module: "M1") },
                contents: withMissingContent
                    ? new List<Content> {
                        new Content("Topic A", "item1", ContentStatus.Missing, priority: 5)
                      }
                    : new List<Content>()
            );
        }

        private static Planner CreateValidPlanner(string? defaultBucketName = "Default")
        {
            return new Planner(
                id: "planner1",
                name: "Main Planner",
                buckets: new List<PlannerBucket>
                {
                    new PlannerBucket(id: "b1", name: defaultBucketName ?? "Default", isDefault: true)
                }
            );
        }

        [Fact]
        public void AllocateCardToBucket_ShouldAllocate_ToDefaultBucket_WhenNoMatchFound()
        {
            // Arrange
            var card = CreateValidCard();
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().NotBeNull();
            result!.CardId.Should().Be(card.Id);
            result.PlannerId.Should().Be(planner.Id);
            result.BucketId.Should().Be(planner.Buckets.First().Id);
        }

        [Fact]
        public void AllocateCardToBucket_ShouldReturnNull_WhenAlreadyAllocated()
        {
            // Arrange
            var planner = CreateValidPlanner();
            var bucket = planner.Buckets.First();
            var card = CreateValidCard(bucketId: bucket.Id, planId: planner.Id);
            var subject = CreateValidSubject();

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void AllocateCardToBucket_ShouldThrow_WhenCardHasNoId()
        {
            var card = CreateValidCard(id: null);
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();

            Action act = () => CardAllocationService.AllocateCardToBucket(card, subject, planner);

            act.Should().Throw<ArgumentException>()
               .WithMessage("*CardId*");
        }

        [Fact]
        public void AllocateCardToBucket_ShouldThrow_WhenCardHasNoChecklist()
        {
            var card = new Card(
                title: "Sample",
                hasDescription: false,
                createdBy: "tester",
                planId: "planner1",
                id: "card1",
                checklist: new List<ChecklistItem>()
            );
            var subject = CreateValidSubject();
            var planner = CreateValidPlanner();

            Action act = () => CardAllocationService.AllocateCardToBucket(card, subject, planner);

            act.Should().Throw<ArgumentException>()
               .WithMessage("*checklist*");
        }

        [Fact]
        public void AllocateCardToBucket_ShouldThrow_WhenSubjectHasNoMissingContent()
        {
            var card = CreateValidCard();
            var subject = CreateValidSubject(withMissingContent: false);
            var planner = CreateValidPlanner();

            Action act = () => CardAllocationService.AllocateCardToBucket(card, subject, planner);

            act.Should().Throw<ArgumentException>()
               .WithMessage("*missing content*");
        }

        [Fact]
        public void AllocateCardToBucket_ShouldPickMatchingBucket_IfExists()
        {
            // Arrange
            var card = CreateValidCard();
            var subject = new Subject(
                id: Guid.NewGuid(),
                modelId: null,
                name: "Physics",
                semester: "2025-1",
                status: SubjectStatus.Complete,
                oferts: new List<Ofert> { new Ofert(project: "PRJ", module: "M1") },
                contents: new List<Content> {
                    new Content("Thermodynamics", "chk1", ContentStatus.Missing, priority: 10)
                }
            );
            var planner = new Planner(
                id: "planner1",
                name: "Science",
                buckets: new List<PlannerBucket>
                {
                    new PlannerBucket("b1", "Thermodynamics Plan", false),
                    new PlannerBucket("b2", "Default", true)
                }
            );

            // Act
            var result = CardAllocationService.AllocateCardToBucket(card, subject, planner);

            // Assert
            result.Should().NotBeNull();
            result!.BucketId.Should().Be("b1");
        }
    }
}
