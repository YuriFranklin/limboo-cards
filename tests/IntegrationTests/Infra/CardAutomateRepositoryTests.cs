namespace LimbooCards.IntegrationTests.Infra
{
    public class CardAutomateRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly CardSettings _settings;
        private readonly CardAutomateRepository _repository;

        public CardAutomateRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CardMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
            });

            _mapper = config.CreateMapper();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            _settings = configuration
            .GetSection("Services:Card")
            .Get<CardSettings>()
            ?? throw new InvalidOperationException("Card settings not found in configuration.");

            var httpClient = new HttpClient();

            _repository = new CardAutomateRepository(httpClient, _mapper, Options.Create(_settings));
        }

        [Fact]
        public async Task GetCardById_ShouldReturnCard()
        {
            var cardId = "6uuxN7xpSESpiSy5Jyg7CWQAJt_m";

            var card = await _repository.GetCardByIdAsync(cardId);

            Assert.NotNull(card);
            Assert.Equal(cardId, card.Id);
        }
    }
}