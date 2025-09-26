namespace LimbooCards.UnitTests.Domain.Entities
{
    using LimbooCards.Domain.Entities;
    public class SubjectTests
    {
        [Fact]
        public void Subject_ShouldInitializeProperties()
        {
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

            Assert.NotEqual(Guid.Empty, subject.Id);
            Assert.Equal("Mathematics", subject.Name);
            Assert.Equal("1st", subject.Semester);
            Assert.Equal(SubjectStatus.Incomplete, subject.Status);
            Assert.Equal(2, subject?.Equivalencies?.Count);
            Assert.Equal(2, subject?.Contents?.Count);
            Assert.Equal(2, subject?.Publishers?.Count);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenIdIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.Empty,
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Complete
                );
            });

            Assert.Contains("Id must be set.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "",
                    semester: "1st",
                    status: SubjectStatus.Complete
                );
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenSemesterIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "",
                    status: SubjectStatus.Complete
                );
            });

            Assert.Contains("Semester cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenStatusIsInvalid()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var ofert = new Ofert("DIG", "AE");

                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    oferts: new List<Ofert> { ofert },
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: (SubjectStatus)999
                );
            });

            Assert.Contains("Invalid status value.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenEquivalenciesContainEmptyValue()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var ofert = new Ofert("DIG", "AE");

                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Incomplete,
                    equivalencies: new List<string> { "Equiv1", "" },
                    oferts: new List<Ofert> { ofert }
                );
            });

            Assert.Contains("Equivalencies cannot contain empty values.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenContentsContainNull()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var ofert = new Ofert("DIG", "AE");

                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Incomplete,
                    oferts: new List<Ofert> { ofert },
                    contents: new List<Content> { null! }
                );
            });

            Assert.Contains("Contents cannot contain null values.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenPublishersContainMoreThanTwo()
        {
            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA"),
                new("PublisherB"),
                new("PublisherC")
            };

            var ofert = new Ofert("DIG", "AE");

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Incomplete,
                    publishers: publishers,
                    oferts: new List<Ofert> { ofert }
                );
            });

            Assert.Contains("Publishers cannot contain more than 2 items.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenMultipleCurrentPublishers()
        {
            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isCurrent: true),
                new("PublisherB", isCurrent: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Incomplete,
                    publishers: publishers,
                    oferts: new List<Ofert> { ofert }
                );
            });

            Assert.Contains("Only one publisher can be marked as current.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Subject_ShouldThrow_WhenMultipleExpectPublishers()
        {
            var publishers = new List<SubjectPublisher>
            {
                new("PublisherA", isExpect: true),
                new("PublisherB", isExpect: true)
            };

            var ofert = new Ofert("DIG", "AE");

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var subject = new Subject(
                    id: Guid.CreateVersion7(),
                    modelId: null,
                    name: "Math",
                    semester: "1st",
                    status: SubjectStatus.Incomplete,
                    publishers: publishers,
                    oferts: new List<Ofert> { ofert }
                );
            });

            Assert.Contains("Only one publisher can be marked as expected.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
