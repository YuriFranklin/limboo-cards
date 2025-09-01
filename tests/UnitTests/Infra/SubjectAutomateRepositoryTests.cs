namespace LimbooCards.UnitTests.Infra
{
    public class SubjectAutomateRepositoryTests
    {
        [Fact]
        public async Task GetSubjectByIdAsync_ShouldReturnMappedSubject_WhenDtoExists()
        {
            var subjectId = Guid.NewGuid();

            var userId = Guid.NewGuid();

            var dto = new SubjectAutomateDto
            {
                UUID = subjectId.ToString(),
                DISCIPLINA = "Matemática",
                STATUS_DIG = "OK",
                OFERTAS = "[DIG - D]",
                EDITORA_NASA = "FAEL",
                EDITORA_MASTER = "",
                EQUIVALENCIA = "Introdução à Matemática",
                OWNERS = new List<UserAutomateDto>
                {
                    new UserAutomateDto
                    {
                        ID = userId.ToString(),
                        FULLNAME = "João Silva",
                        EMAIL = "user@test.com"
                    }
                },
                PUBLISHERS = new List<SubjectPublisherAutomateDto>
                {
                    new SubjectPublisherAutomateDto
                    {
                        NOME = "FAEL",
                        IS_CURRENT = true,
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
                        req.RequestUri!.ToString().EndsWith($"/subjects/{subjectId}")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(dto)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var expectedSubject = new Subject(
                id: subjectId,
                name: "Matemática",
                semester: "20252",
                status: SubjectStatus.Complete,
                oferts: [new("DIG", "D")],
                equivalencies: ["Introdução à Matemática"],
                contents: [],
                owner: new User(
                        id: userId,
                        fullName: "João Silva",
                        email: "user@test.com"
                    ),
                coOwners: [],
                publishers: [new SubjectPublisher(name: "FAEL", isExpect: false, isCurrent: true)]
            );

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContentMappingProfile>();
                cfg.AddProfile<SubjectMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<SubjectPublisherMappingProfile>();
            });

            var mapper = configuration.CreateMapper();

            var repository = new SubjectAutomateRepository(httpClient, mapper);

            var result = await repository.GetSubjectByIdAsync(subjectId);

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedSubject);
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

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var mapperMock = new Mock<IMapper>();
            var repository = new SubjectAutomateRepository(httpClient, mapperMock.Object);

            var result = await repository.GetSubjectByIdAsync(subjectId);

            Assert.Null(result);
            mapperMock.Verify(m => m.Map<Subject>(It.IsAny<SubjectAutomateDto>()), Times.Never);
        }
    }

}