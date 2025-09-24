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
    }
}
