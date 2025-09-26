namespace LimbooCards.UnitTests.Domain.Services
{
    public class PinEvaluatorServiceTests
    {
        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenNoRules()
        {
            var planner = new Planner(Guid.CreateVersion7().ToString(), "Planner 1", new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) }, null);
            var contents = new List<Content>
            {
                new("Content1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };

            var equivalencies = new List<string> { "Equiv1", "Equiv2" };
            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isCurrent: true),
                new("PublisherB", isExpect: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Mathematics",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: equivalencies,
                contents: contents,
                publishers: publishers,
                oferts: new List<Ofert> { ofert }
            );

            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.CreateVersion7().ToString());

            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleMatchesContentName()
        {
            var contents = new List<Content>
            {
                new("Unity 1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };

            var equivalencies = new List<string> { "Equiv1", "Equiv2" };

            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isCurrent: true),
                new("PublisherB", isExpect: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Mathematics Geometric",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: equivalencies,
                contents: contents,
                publishers: publishers,
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1'", PinColor.Blue);
            var planner = new Planner(Guid.CreateVersion7().ToString(), "Planner 1", new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) }, new List<PinRule> { rule });

            var cardId = Guid.CreateVersion7().ToString();
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, cardId);

            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinColor.Blue.ToString());
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleDoesNotMatch()
        {
            var contents = new List<Content>
            {
                new("Unity 1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };

            var equivalencies = new List<string> { "Equiv1", "Equiv2" };

            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isCurrent: true),
                new("PublisherB", isExpect: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var content = new Content("Geometry", "Checklist1", ContentStatus.Missing, 1);
            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Mathematics Geometric",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: equivalencies,
                contents: contents,
                publishers: publishers,
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Algebra'", PinColor.Blue);
            var planner = new Planner(Guid.CreateVersion7().ToString(), "Planner 1", new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) }, new List<PinRule> { rule });

            var cardId = Guid.CreateVersion7().ToString();
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, cardId);

            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyMultiplePins_WhenMultipleRulesMatch()
        {
            var contents = new List<Content>
            {
                new("Unity 1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };

            var equivalencies = new List<string> { "Equiv1", "Equiv2" };

            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isCurrent: true),
                new("PublisherB", isExpect: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var content = new Content("Geometry", "Checklist1", ContentStatus.Missing, 1);
            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Mathematics Geometric",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: equivalencies,
                contents: contents,
                publishers: publishers,
                oferts: new List<Ofert> { ofert }
            );

            var rules = new List<PinRule>
                        {
                            new PinRule("Subject.Contents.Name contains 'Unity 1'", PinColor.Blue),
                            new PinRule("Subject.Name contains 'Math'", PinColor.Green)
                        };

            var planner = new Planner(Guid.CreateVersion7().ToString(), "Planner 1", new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) }, rules);

            var cardId = Guid.CreateVersion7().ToString();
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, cardId);

            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKeys(PinColor.Blue.ToString(), PinColor.Green.ToString());
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleWithAndMatches()
        {
            var contents = new List<Content>
            {
                new("Unity 1", "Checklist1", ContentStatus.OK),
                new("Content2", "Checklist2", ContentStatus.OK)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Mathematics Geometric",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: new List<string> { "Equiv1" },
                contents: contents,
                publishers: new List<SubjectPublisher>(),
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' AND Subject.Name contains 'Math'", PinColor.Red);

            var planner = new Planner(
                Guid.CreateVersion7().ToString(), "Planner 1",
                new List<PlannerBucket> { new(Guid.CreateVersion7().ToString(), "Default", true) },
                new List<PinRule> { rule }
            );

            var cardId = Guid.CreateVersion7().ToString();
            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, cardId);

            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinColor.Red.ToString());
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleWithAndDoesNotMatch()
        {
            var contents = new List<Content>
            {
                new("Unity 1", "Checklist1", ContentStatus.OK)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "History",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: new List<string>(),
                contents: contents,
                publishers: new List<SubjectPublisher>(),
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' AND Subject.Name contains 'Math'", PinColor.Red);

            var planner = new Planner(
                Guid.CreateVersion7().ToString(), "Planner 1",
                new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) },
                new List<PinRule> { rule }
            );

            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.CreateVersion7().ToString());

            result.Should().BeNull();
        }

        [Fact]
        public void EvaluateCardPins_ShouldApplyPin_WhenRuleWithOrMatches()
        {
            var contents = new List<Content>
            {
                new("Geometry", "Checklist1", ContentStatus.OK)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "History",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: new List<string>(),
                contents: contents,
                publishers: new List<SubjectPublisher>(),
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' OR Subject.Name contains 'History'", PinColor.Yellow);

            var planner = new Planner(
                Guid.CreateVersion7().ToString(), "Planner 1",
                new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) },
                new List<PinRule> { rule }
            );

            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.CreateVersion7().ToString());

            result.Should().NotBeNull();
            result!.AppliedCategories.Should().ContainKey(PinColor.Yellow.ToString());
        }

        [Fact]
        public void EvaluateCardPins_ShouldReturnNull_WhenRuleWithOrDoesNotMatch()
        {
            var contents = new List<Content>
            {
                new("Geometry", "Checklist1", ContentStatus.OK)
            };

            var ofert = new Ofert("DIG", "AE");

            var subject = new Subject(
                id: null,
                modelId: null,
                name: "Biology",
                semester: "1st",
                status: SubjectStatus.Incomplete,
                equivalencies: new List<string>(),
                contents: contents,
                publishers: new List<SubjectPublisher>(),
                oferts: new List<Ofert> { ofert }
            );

            var rule = new PinRule("Subject.Contents.Name contains 'Unity 1' OR Subject.Name contains 'Math'", PinColor.Purple);

            var planner = new Planner(
                Guid.CreateVersion7().ToString(), "Planner 1",
                new List<PlannerBucket> { new PlannerBucket(Guid.CreateVersion7().ToString(), "Default", true) },
                new List<PinRule> { rule }
            );

            var result = PinEvaluatorService.EvaluateCardPins(subject, planner, Guid.CreateVersion7().ToString());

            result.Should().BeNull();
        }

    }
}