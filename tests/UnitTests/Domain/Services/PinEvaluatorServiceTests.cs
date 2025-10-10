namespace LimbooCards.UnitTests.Domain.Services
{
    public class PinEvaluatorServiceTests
    {
        private static Planner CreateValidPlanner(List<PinRule>? rules = null)
        {
            var validBuckets = new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "Default Bucket", isDefault: true),
                new(Guid.NewGuid().ToString(), "End Bucket", isEnd: true),
                new(Guid.NewGuid().ToString(), "History Bucket", isHistory: true)
            };

            return new Planner(Guid.NewGuid().ToString(), "Test Planner", validBuckets, rules);
        }

        private static Subject CreateTestSubject(string name = "Mathematics Geometric", List<Content>? contents = null)
        {
            return new Subject(
                id: null,
                modelId: null,
                name: name,
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: new List<string> { "Equiv1" },
                contents: contents ?? new List<Content> { new("Unity 1", "Checklist1", ContentStatus.OK) },
                publishers: new List<SubjectPublisher> { new("PublisherA", true) },
                oferts: new List<Ofert> { new("DIG", "AE") }
            );
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenNoRules()
        {
            // Arrange
            var planner = CreateValidPlanner(rules: null);
            var subject = CreateTestSubject();

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleMatchesContentName()
        {
            // Arrange
            var subject = CreateTestSubject();
            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1'", PinColor.Blue);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinEvaluatorService.GetEnumMemberValue(PinColor.Blue));
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleDoesNotMatch()
        {
            // Arrange
            var subject = CreateTestSubject();
            var rule = new PinRule("Subject.Contents.Name contains 'Algebra'", PinColor.Blue);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyMultiplePins_WhenMultipleRulesMatch()
        {
            // Arrange
            var subject = CreateTestSubject(name: "Mathematics");
            var rules = new List<PinRule>
            {
                new("Subject.Contents.Name contains 'Unity 1'", PinColor.Blue),
                new("Subject.Name contains 'Math'", PinColor.Green)
            };
            var planner = CreateValidPlanner(rules);

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKeys(PinEvaluatorService.GetEnumMemberValue(PinColor.Blue), PinEvaluatorService.GetEnumMemberValue(PinColor.Green));
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleWithAndMatches()
        {
            // Arrange
            var subject = CreateTestSubject(name: "Mathematics");
            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' AND Subject.Name contains 'Math'", PinColor.Red);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinEvaluatorService.GetEnumMemberValue(PinColor.Red));
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleWithAndDoesNotMatch()
        {
            // Arrange
            var subject = CreateTestSubject(name: "History");
            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' AND Subject.Name contains 'Math'", PinColor.Red);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleWithOrMatches()
        {
            // Arrange
            var subject = CreateTestSubject(name: "History");
            var rule = new PinRule("Subject.Contents.Name contains 'Algebra' OR Subject.Name contains 'History'", PinColor.Yellow);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinEvaluatorService.GetEnumMemberValue(PinColor.Yellow));
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleWithOrDoesNotMatch()
        {
            // Arrange
            var subject = CreateTestSubject(name: "Biology");
            var rule = new PinRule("Subject.Contents.Name contains 'Algebra' OR Subject.Name contains 'History'", PinColor.Purple);
            var planner = CreateValidPlanner(new List<PinRule> { rule });

            // Act
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.NewGuid().ToString());

            // Assert
            result.Should().BeNull();
        }
    }
}