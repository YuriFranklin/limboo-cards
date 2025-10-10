

namespace LimbooCards.UnitTests.Domain.Services
{
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Services;

    public class CardChecklistNormalizeServiceTests
    {
        private readonly Card _card;

        public CardChecklistNormalizeServiceTests()
        {
            var updatedBy = Guid.CreateVersion7().ToString();
            var updatedAt = DateTime.UtcNow;

            _card = new Card(
                title: "Plano de Estudos",
                hasDescription: true,
                createdBy: "user1",
                planId: "plan-123",
                id: "card-1",
                checklist: new List<ChecklistItem>
                {
                    new(
                        id: "chk1",
                        title: "Unidade I - Vídeo introdutório",
                        isChecked: false,
                        orderHint: "A",
                        updatedAt: updatedAt,
                        updatedBy: updatedBy
                    ),
                    new(
                        id: "chk2",
                        title: "Scorm 3",
                        isChecked: false,
                        orderHint: "B",
                        updatedAt: updatedAt,
                        updatedBy: updatedBy
                    ),
                    new(
                        id: "chk3",
                        title: "Item que não bate com nada",
                        isChecked: false,
                        orderHint: "C",
                        updatedAt: updatedAt,
                        updatedBy: updatedBy
                    )
                }
            );
        }

        [Fact]
        public void NormalizeTokens_ShouldNormalizeProperly()
        {
            var tokens = CardChecklistNormalizeService.NormalizeTokens("Unidade I de Vídeo");
            Assert.Contains("unidade", tokens);
            Assert.Contains("1", tokens);
            Assert.Contains("videoteca", tokens);
            Assert.DoesNotContain("de", tokens);
        }

        [Fact]
        public void NormalizeChecklist_ShouldReturnNormalizedItems_WhenScoreIsAboveThreshold()
        {

            // Act
            var normalized = CardChecklistNormalizeService.NormalizeChecklist(_card);

            // Assert
            Assert.NotEmpty(normalized);
            Assert.All(normalized, n => Assert.False(string.IsNullOrWhiteSpace(n.NormalizedTitle)));
            Assert.Contains(normalized, n => n.ChecklistItemId == "chk1");
            Assert.Contains(normalized, n => n.ChecklistItemId == "chk2");

            Assert.DoesNotContain(normalized, n => n.ChecklistItemId == "chk3");
        }

        [Fact]
        public void NormalizeChecklist_ShouldReturnEmpty_WhenChecklistIsNull()
        {
            var cardWithoutChecklist = _card.With(checklist: new List<ChecklistItem>());
            var result = CardChecklistNormalizeService.NormalizeChecklist(cardWithoutChecklist);
            Assert.Empty(result);
        }
    }
}
