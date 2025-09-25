namespace LimbooCards.UnitTests.Application
{
    public class CardApplicationServiceTests
    {
        private readonly Mock<ICardRepository> cardRepositoryMock;
        private readonly Mock<ISubjectRepository> subjectRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock;
        private readonly CardApplicationService service;

        public CardApplicationServiceTests()
        {
            cardRepositoryMock = new Mock<ICardRepository>();
            mapperMock = new Mock<IMapper>();
            service = new CardApplicationService(cardRepositoryMock.Object, subjectRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateCardAsync_ShouldAddCardAndReturn_CardDto()
        {
            var dto = new CreateCardDto
            {
                Title = "Card 1",
                CreatedBy = Guid.CreateVersion7().ToString(),
                Description = "Description"
            };
            var card = new Card(title: dto.Title, description: dto.Description, hasDescription: false, createdBy: dto.CreatedBy);
            var cardDto = new CardDto
            {
                Title = card.Title,
                CreatedBy = card.CreatedBy,
                Description = card.Description
            };

            mapperMock.Setup(m => m.Map<Card>(dto)).Returns(card);
            mapperMock.Setup(m => m.Map<CardDto>(card)).Returns(cardDto);

            var result = await service.CreateCardAsync(dto);

            cardRepositoryMock.Verify(r => r.AddCardAsync(card), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(card.Title, result.Title);
            Assert.Equal(card.Description, result.Description);
            Assert.Equal(card.CreatedBy, result.CreatedBy);
        }

        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnMappedCardDto()
        {
            var cardId = Guid.CreateVersion7().ToString();
            var card = new Card(title: "Title", description: "Desc", hasDescription: true, createdBy: Guid.CreateVersion7().ToString());
            var cardDto = new CardDto { Id = card.Id, Title = card.Title };

            cardRepositoryMock.Setup(r => r.GetCardByIdAsync(cardId)).ReturnsAsync(card);
            mapperMock.Setup(m => m.Map<CardDto>(card)).Returns(cardDto);

            var result = await service.GetCardByIdAsync(cardId);

            Assert.Equal(cardDto, result);
        }

        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnNull_WhenCardNotFound()
        {
            var cardId = Guid.CreateVersion7().ToString();
            cardRepositoryMock.Setup(r => r.GetCardByIdAsync(cardId)).ReturnsAsync((Card?)null!);

            var result = await service.GetCardByIdAsync(cardId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCardsAsync_ShouldReturnMappedCardDtoList()
        {
            var cards = new List<Card>
            {
                new Card(title: "Title1", description: "Desc", hasDescription: true, createdBy: Guid.CreateVersion7().ToString()),
                new Card(title: "Title2", description: "Desc", hasDescription: true, createdBy: Guid.CreateVersion7().ToString()),
            };
            var cardsDto = new List<CardDto>
            {
                new CardDto { Id = cards[0].Id, Title = cards[0].Title },
                new CardDto { Id = cards[1].Id, Title = cards[1].Title }
            };

            cardRepositoryMock.Setup(r => r.GetAllCardsAsync()).ReturnsAsync(cards);
            mapperMock.Setup(m => m.Map<IEnumerable<CardDto>>(cards)).Returns(cardsDto);

            var result = await service.GetAllCardsAsync();

            Assert.Equal(cardsDto, result);
        }

        [Fact]
        public async Task UpdateCardAsync_ShouldMapDtoAndCallRepository()
        {
            var dto = new UpdateCardDto { Id = Guid.CreateVersion7().ToString(), Title = "Updated" };
            var card = new Card(title: dto.Title, description: string.Empty, hasDescription: false, createdBy: Guid.CreateVersion7().ToString());

            mapperMock.Setup(m => m.Map<Card>(dto)).Returns(card);

            await service.UpdateCardAsync(dto);

            cardRepositoryMock.Verify(r => r.UpdateCardAsync(card), Times.Once);
        }

        [Fact]
        public async Task DeleteCardAsync_ShouldCallRepositoryWithCorrectId()
        {
            var cardId = Guid.CreateVersion7().ToString();

            await service.DeleteCardAsync(cardId);

            cardRepositoryMock.Verify(r => r.DeleteCardAsync(cardId), Times.Once);
        }

        [Fact]
        public async Task NormalizeCardChecklistAsync_WhenCardExists_ShouldReturnNormalizedItems()
        {
            // Arrange
            var cardId = "card1";
            var checklistItem = new ChecklistItem(
                id: "item1",
                title: "Material da Unidade I",
                isChecked: false,
                orderHint: "a",
                updatedAt: DateTime.UtcNow,
                updatedBy: "user1"
            );

            var card = new Card(
                id: cardId,
                title: "Test Card",
                hasDescription: false,
                createdBy: "user1",
                checklist: new List<ChecklistItem> { checklistItem }
            );

            var expectedDtos = new List<ChecklistItemNormalizedDto>
            {
                new() { ChecklistItemId = "item1", NormalizedTitle = "[MATERIAL_1] Material Didático da Unidade 1" }
            };

            cardRepositoryMock.Setup(r => r.GetCardByIdAsync(cardId)).ReturnsAsync(card);

            mapperMock.Setup(m => m.Map<IEnumerable<ChecklistItemNormalizedDto>>(It.IsAny<IEnumerable<ChecklistItemNormalized>>()))
                       .Returns(expectedDtos);

            // Act
            var result = await service.NormalizeCardChecklistAsync(cardId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtos);
            cardRepositoryMock.Verify(r => r.GetCardByIdAsync(cardId), Times.Once);
        }

        [Fact]
        public async Task NormalizeCardChecklistAsync_WhenCardNotFound_ShouldReturnNull()
        {
            var cardId = "non-existent-card";
            cardRepositoryMock.Setup(r => r.GetCardByIdAsync(cardId)).ReturnsAsync((Card?)null);

            // Act
            var result = await service.NormalizeCardChecklistAsync(cardId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task NormalizeCardChecklistAsync_WhenNormalizationFails_ShouldThrowException()
        {
            // Arrange
            var cardId = "card1";
            var checklistItem = new ChecklistItem("item1", "Non-matchable gibberish", false, "a", DateTime.UtcNow, "user1");
            var card = new Card("Test Card", false, "user1", null, cardId, checklist: new List<ChecklistItem> { checklistItem });

            cardRepositoryMock.Setup(r => r.GetCardByIdAsync(cardId)).ReturnsAsync(card);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.NormalizeCardChecklistAsync(cardId));
        }
    }
}
