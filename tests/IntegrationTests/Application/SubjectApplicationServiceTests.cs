namespace LimbooCards.IntegrationTests.Application
{
    public class SubjectApplicationServiceTests
    {
        private readonly SubjectApplicationService _service;
        public SubjectApplicationServiceTests()
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

            var subjectSettings = configuration
            .GetSection("Services:Subject")
            .Get<SubjectSettings>()
            ?? throw new InvalidOperationException("Subject settings not found in configuration.");

            var cardSettings = configuration
            .GetSection("Services:Card")
            .Get<CardSettings>()
            ?? throw new InvalidOperationException("Card settings not found in configuration.");

            var httpClient = new HttpClient();

            var subjectRepository = new SubjectAutomateRepository(httpClient, _mapper, Options.Create(subjectSettings));

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            var context = new AppDbContext(options);

            var cardRepository = new CardAutomateRepository(httpClient, _mapper, Options.Create(cardSettings));
            var plannerRepository = new PlannerDbRepository(context, _mapper);

            var userRepository = new UserAutomateRepository(httpClient, _mapper);

            _service = new SubjectApplicationService(subjectRepository, userRepository, plannerRepository, cardRepository, _mapper);
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldCreateNewCard_WhenSubjectAndPlannerExist()
        {
            // Arrange
            var subjectId = Guid.Parse("01997738-2b96-7d0e-be93-85471d961fb3");
            var plannerId = "r69txn4te023WTwwj8jHL2QAFVIN";

            // Act
            var result = await _service.EnsureCardForSubject(plannerId, subjectId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}