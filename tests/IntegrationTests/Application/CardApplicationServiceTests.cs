namespace LimbooCards.IntegrationTests.Application
{
    public class CardApplicationServiceTests
    {
        private readonly CardApplicationService _service;
        public CardApplicationServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LimbooCards.Application.Mappings.CardMappingProfile>();
                cfg.AddProfile<CardMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
                cfg.AddProfile<SubjectMappingProfile>();
                cfg.AddProfile<SubjectPublisherMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
                cfg.AddProfile<ContentMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });

            var _mapper = config.CreateMapper();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var cardSettings = configuration
            .GetSection("Services:Card")
            .Get<CardSettings>()
            ?? throw new InvalidOperationException("Card settings not found in configuration.");

            var httpClient = new HttpClient();

            var cardRepository = new CardAutomateRepository(httpClient, _mapper, Options.Create(cardSettings));

            var subjectSettings = configuration
            .GetSection("Services:Subject")
            .Get<SubjectSettings>()
            ?? throw new InvalidOperationException("Subject settings not found in configuration.");

            var subjectRepository = new SubjectAutomateRepository(httpClient, _mapper, Options.Create(subjectSettings));

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            var context = new AppDbContext(options);

            var plannerRepository = new PlannerDbRepository(context, _mapper);

            _service = new CardApplicationService(cardRepository, subjectRepository, plannerRepository, _mapper);
        }

        [Fact(Timeout = 120000)]
        public async Task NormalizeCardsAsync_ShouldReturnNormalizedCards()
        {
            var cardIds = new List<string>(["wazJJyHvIUCM2MyXg6PFaWQAOEcZ"]);

            var result = await _service.NormalizeCardsAsync(cardIds);

            Assert.Single(result.Success!);
            Assert.Empty(result.Failed!);
        }
    }
}