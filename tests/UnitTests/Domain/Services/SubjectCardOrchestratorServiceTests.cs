namespace LimbooCards.UnitTests.Domain.Services
{
    public class SubjectCardOrchestratorServiceTests
    {

        private static Subject CreateTestSubject(List<Content>? contents)
        {
            var owner = new User(
                id: Guid.NewGuid().ToString(),
                name: "John",
                fullName: "John Doe",
                email: "test@test.com"
                );

            return new Subject(
                id: Guid.NewGuid(),
                modelId: "model-abc",
                name: "Mathematics",
                semester: "2025-1",
                status: SubjectStatus.Incomplete,
                contents: contents,
                owner: owner,
                oferts: new List<Ofert> { new("DIG", "A") }
            );
        }

        private static Planner CreateTestPlanner()
        {
            var buckets = new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "Default", isDefault: true, isEnd: false, isHistory: false),
                new(Guid.NewGuid().ToString(), "Done", isDefault: false, isEnd: true, isHistory: false),
                new(Guid.NewGuid().ToString(), "Archived", isDefault: false, isEnd: false, isHistory: true)
            };
            return new Planner("planner-xyz", "Test Planner", buckets);
        }

        [Fact]
        public void EnsureCardForSubject_ShouldReturnCard_WhenContentIsMissing()
        {
            var contents = new List<Content>
            {
                new("Content OK", "Title for OK", ContentStatus.OK),
                new("Content Missing 1", "Title for Missing 1", ContentStatus.Missing),
                new("Content Missing 2", "Title for Missing 2", ContentStatus.Missing)
            };
            var subject = CreateTestSubject(contents);
            var planner = CreateTestPlanner();

            // Act
            var card = SubjectCardOrchestratorService.EnsureCardForSubject(subject, planner);

            // Assert
            card.Should().NotBeNull();
            card!.PlanId.Should().Be(planner.Id);
            card.CreatedBy.Should().Be(subject.Owner!.Id);
            card.Checklist.Should().HaveCount(2);

            card!.Checklist![0].Title.Should().Be("Title for Missing 1");
            card.Checklist[0].Id.Should().Be("2");

            card.Checklist[1].Title.Should().Be("Title for Missing 2");
            card.Checklist[1].Id.Should().Be("3");
        }

        [Fact]
        public void EnsureCardForSubject_ShouldReturnNull_WhenContentsAreNull()
        {
            // Arrange
            var subject = CreateTestSubject(contents: null);
            var planner = CreateTestPlanner();

            // Act
            var card = SubjectCardOrchestratorService.EnsureCardForSubject(subject, planner);

            // Assert
            card.Should().BeNull();
        }

        [Fact]
        public void EnsureCardForSubject_ShouldReturnNull_WhenContentsAreEmpty()
        {
            // Arrange
            var subject = CreateTestSubject(contents: new List<Content>());
            var planner = CreateTestPlanner();

            // Act
            var card = SubjectCardOrchestratorService.EnsureCardForSubject(subject, planner);

            // Assert
            card.Should().BeNull();
        }

        [Fact]
        public void EnsureCardForSubject_ShouldReturnNull_WhenNoContentIsMissing()
        {
            // Arrange
            var contents = new List<Content>
            {
                new("Content1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };
            var subject = CreateTestSubject(contents);
            var planner = CreateTestPlanner();

            // Act
            var card = SubjectCardOrchestratorService.EnsureCardForSubject(subject, planner);

            // Assert
            card.Should().BeNull();
        }
    }
}