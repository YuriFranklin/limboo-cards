namespace LimbooCards.UnitTests.Domain.Services
{
    using LimbooCards.Domain.Services;
    using LimbooCards.Domain.Entities;

    public class CardSubjectMatcherServiceTests
    {
        [Fact]
        public void MatchSubjectForCardAsync_ShouldReturnExistingSubject_WhenCardHasValidSubjectId()
        {
            // Arrange
            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(id: Guid.NewGuid(), modelId: "123", name: "Matemática", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "Qualquer título",
                subjectId: subject.Id,
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, [subject]);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subject.Id, result!.Id);
        }

        [Fact]
        public void MatchSubjectForCardAsync_ShouldReturnNull_WhenCardHasSubjectIdButNotFound()
        {
            // Arrange
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "Qualquer título",
                subjectId: Guid.NewGuid(),
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );
            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(id: Guid.NewGuid(), modelId: "123", name: "Matemática", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });

            var subjects = new[]
            {
                subject
            };

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, subjects);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MatchSubjectForCardAsync_ShouldReturnBestMatch_ByJaccardSimilarity()
        {
            // Arrange
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "Matematica Basica",
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );

            var ofert = new Ofert("DIG", "AE");

            var s1 = new Subject(id: Guid.NewGuid(), modelId: "123", name: "Matemática Básica", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });
            var s2 = new Subject(id: Guid.NewGuid(), modelId: "122", name: "História Geral", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });
            var s3 = new Subject(id: Guid.NewGuid(), modelId: "121", name: "Física Quântica", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });

            var subjects = new[] { s1, s2, s3 };

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, subjects);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(s1.Id, result!.Id);
        }

        [Fact]
        public void MatchSubjectForCardAsync_ShouldReturnNull_WhenSimilarityIsBelowThreshold()
        {
            // Arrange
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "Palavra Inexistente",
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );

            var ofert = new Ofert("DIG", "AE");
            List<Subject> subjects = [new(id: Guid.NewGuid(), modelId: "123", name: "Matemática Básica", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert })];

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, subjects);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MatchSubjectForCardAsync_ShouldNormalizeNumbersAndRomans()
        {
            // Arrange
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "Disciplina IV",
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );

            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(id: Guid.NewGuid(), modelId: "123", name: "Discíplina 4", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, [subject]);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subject.Id, result!.Id);
        }

        [Fact]
        public void MatchSubjectForCardAsync_ShouldIgnoreStopWords()
        {
            // Arrange
            var card = new Card(
                id: Guid.NewGuid().ToString(),
                title: "A Historia do Brasil",
                hasDescription: false,
                createdBy: Guid.NewGuid().ToString(),
                planId: Guid.NewGuid().ToString()
            );

            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(id: Guid.NewGuid(), modelId: "123", name: "História Brasil", semester: "20252", status: SubjectStatus.Complete, oferts: new List<Ofert> { ofert });

            // Act
            var result = CardSubjectMatcherService.MatchSubjectForCardAsync(card, [subject]);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subject.Id, result!.Id);
        }
    }
}
