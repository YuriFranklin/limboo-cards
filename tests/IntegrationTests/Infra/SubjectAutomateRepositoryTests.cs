namespace LimbooCards.IntegrationTests.Infra
{
    public class SubjectAutomateRepositoryTests
    {
        private readonly SubjectAutomateRepository _repository;

        public SubjectAutomateRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubjectMappingProfile>();
                cfg.AddProfile<SubjectPublisherMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
                cfg.AddProfile<ContentMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });

            var mapper = config.CreateMapper();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var settings = configuration
            .GetSection("Services:Subject")
            .Get<SubjectSettings>()
            ?? throw new InvalidOperationException("Subject settings not found in configuration.");

            var httpClient = new HttpClient();

            _repository = new SubjectAutomateRepository(httpClient, mapper, Options.Create(settings));
        }
        [Fact]
        public async Task GetAllSubjectsAsync_WhenApiReturnsSubjects_ShouldReturnSubjectList()
        {
            var subjects = await _repository.GetAllSubjectsAsync();
            Assert.IsType<IEnumerable<Subject>>(subjects, exactMatch: false);
        }

        [Fact]
        public async Task GetSubjectByIdAsync_WhenApiReturnsSubject_ShouldReturnSubject()
        {
            var subjectId = Guid.Parse("0199c9d4-5f4b-7a26-a3ce-4ce29e37c850");

            var subject = await _repository.GetSubjectByIdAsync(subjectId);

            Assert.NotNull(subject);
            Assert.Equal(subjectId, subject.Id);
        }
    }

}