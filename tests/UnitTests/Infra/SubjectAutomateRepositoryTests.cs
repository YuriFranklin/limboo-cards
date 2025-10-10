namespace LimbooCards.UnitTests.Infra
{
    public class SubjectAutomateRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly IOptions<SubjectSettings> _options;

        public SubjectAutomateRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubjectMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<SubjectPublisherMappingProfile>();
            });

            _mapper = config.CreateMapper();

            var settings = new SubjectSettings
            {
                GetAllUrl = "http://localhost/subjects?api-version=1",
                GetByIdUrl = "http://localhost/subjects?api-version=1",
                GetPagedUrl = "http://localhost/subjects?api-version=1"
            };

            _options = Options.Create(settings);
        }

        [Fact]
        public async Task GetSubjectByIdAsync_ShouldReturnMappedSubject_WhenDtoExists()
        {
            var subjectId = Guid.Parse("86d87fc0-9f2e-4a3b-a750-bf752645080a");
            var ownerId = "17115131-c022-4c9b-a284-29ccf79f5b93";
            var dto = new SubjectAutomateDto
            {
                Id = "9875",
                Disciplina = "Adaptação e Flexibilização Curricular",
                Equivalencia = "",
                EditoraMaster = "",
                EditoraNasa = "FAEL",
                Ofertas = "[DIG - D]",
                Status = "OK",
                Uuid = subjectId.ToString(),
                Owners = new List<UserAutomateDto>
                {
                    new UserAutomateDto
                    {
                        ID = ownerId,
                        NAME = "DHIEGO",
                        FULLNAME = "DHIEGO",
                        EMAIL = "teste@test.com"
                    }
                },
                Publishers = new List<SubjectPublisherAutomateDto>
                {
                    new SubjectPublisherAutomateDto
                    {
                        NOME = "FAEL",
                        IS_EXPECTED = true
                    }
                }
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.ToString().StartsWith(_options.Value.GetByIdUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(dto)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var repository = new SubjectAutomateRepository(httpClient, _mapper, _options);

            var result = await repository.GetSubjectByIdAsync(subjectId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(subjectId);
            result.Name.Should().Be("Adaptação e Flexibilização Curricular");
        }

        [Fact]
        public async Task GetSubjectByIdAsync_ShouldReturnNull_WhenApiReturnsNull()
        {
            var subjectId = Guid.NewGuid();

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create<SubjectAutomateDto?>(null)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var mapperMock = new Mock<IMapper>();
            var repository = new SubjectAutomateRepository(httpClient, mapperMock.Object, _options);

            var result = await repository.GetSubjectByIdAsync(subjectId);

            result.Should().BeNull();
            mapperMock.Verify(m => m.Map<Subject>(It.IsAny<SubjectAutomateDto>()), Times.Never);
        }
    }
}
