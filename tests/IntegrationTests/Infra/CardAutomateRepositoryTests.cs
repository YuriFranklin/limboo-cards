namespace LimbooCards.IntegrationTests.Infra
{
    public class CardAutomateRepositoryTests
    {
        private readonly CardAutomateRepository _repository;

        public CardAutomateRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CardMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
            });

            var mapper = config.CreateMapper();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var settings = configuration
            .GetSection("Services:Card")
            .Get<CardSettings>()
            ?? throw new InvalidOperationException("Card settings not found in configuration.");

            var httpClient = new HttpClient();

            _repository = new CardAutomateRepository(httpClient, mapper, Options.Create(settings));
        }

        [Fact]
        public async Task GetCardById_ShouldReturnCard()
        {
            var cardId = "0YuzjRsvl0eYXhJX-2QIcGQAPXtr";

            var card = await _repository.GetCardByIdAsync(cardId);

            Assert.NotNull(card);
            Assert.Equal(cardId, card.Id);
        }

        [Fact]
        public async Task GetCardByIdAsync_WhenApiReturnsNotFound_ShouldThrowHttpRequestException()
        {
            var cardId = "non-existent-id";
            await Assert.ThrowsAsync<HttpRequestException>(async () => await _repository.GetCardByIdAsync(cardId));
        }

        [Fact]
        public async Task GetAllCardsAsync_WhenApiReturnsCards_ShouldReturnCardList()
        {
            var cards = await _repository.GetAllCardsAsync();
            Assert.IsType<IEnumerable<Card>>(cards, exactMatch: false);
        }
    }
}