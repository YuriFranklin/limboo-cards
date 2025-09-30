namespace LimbooCards.UnitTests.Domain.Services
{
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Services;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class CardTitleNormalizeServiceTests
    {
        [Fact]
        public void Normalize_ShouldIncludeModelIdAndUppercaseName()
        {
            // Arrange
            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(
                id: Guid.NewGuid(),
                modelId: "123",
                name: "Matemática Financeira",
                semester: "20252",
                status: SubjectStatus.Complete,
                oferts: new List<Ofert> { ofert });

            // Act
            var result = CardTitleNormalizeService.Normalize(subject);

            // Assert
            Assert.Contains("[PENDÊNCIA - 123]", result);
            Assert.Contains("MATEMÁTICA FINANCEIRA", result);
        }

        [Fact]
        public void Normalize_ShouldThrow_WhenSubjectIsNull()
        {
            // Arrange
            Subject? subject = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                CardTitleNormalizeService.Normalize(subject!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Normalize_ShouldThrow_WhenModelIdIsNullOrEmpty(string? modelId)
        {
            // Arrange
            var ofert = new Ofert("DIG", "AE");
            var subject = new Subject(
                id: Guid.NewGuid(),
                modelId: modelId ?? string.Empty,
                name: "Matemática",
                semester: "20252",
                status: SubjectStatus.Complete,
                oferts: new List<Ofert> { ofert });

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                CardTitleNormalizeService.Normalize(subject));
            Assert.Contains("ModelId", ex.Message);
        }
    }
}
