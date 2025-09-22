namespace LimbooCards.UnitTests.Infra
{
    public class SubjectAutomateRepositoryTests
    {
        [Fact]
        public async Task GetSubjectByIdAsync_ShouldReturnMappedSubject_WhenDtoExists()
        {
            var subjectByIdUrl = "http://localhost/subjects?api-version=1";
            Environment.SetEnvironmentVariable("SUBJECT_GETBYID_URL", subjectByIdUrl);

            var subjectId = Guid.Parse("86d87fc0-9f2e-4a3b-a750-bf752645080a");
            var ownerId = "17115131-c022-4c9b-a284-29ccf79f5b93";

            var dto = new SubjectAutomateDto
            {
                ID = "9875",
                DISCIPLINA = "Adaptação e Flexibilização Curricular",
                EQUIVALENCIA = "",
                EDITORA_MASTER = "",
                EDITORA_NASA = "FAEL",
                OFERTAS = "[DIG - D]",
                STATUS_DIG = "OK",
                UUID = subjectId.ToString(),
                OWNERS = new List<UserAutomateDto>
                {
                    new UserAutomateDto
                    {
                        ID = ownerId,
                        FULLNAME = "DHIEGO",
                        EMAIL = "teste@test.com"
                    }
                },
                PUBLISHERS = new List<SubjectPublisherAutomateDto>
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
                        req.RequestUri!.ToString().StartsWith(subjectByIdUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(dto)
                });

            var httpClient = new HttpClient(handlerMock.Object);

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
            result!.Id.Should().Be(subjectId);
            result.Name.Should().Be("Adaptação e Flexibilização Curricular");
        }


        [Fact]
        public async Task GetSubjectByIdAsync_ShouldReturnNull_WhenApiReturnsNull()
        {
            var subjectId = Guid.CreateVersion7();

            var subjectByIdUrl = "http://localhost/subjects?api-version=1";
            Environment.SetEnvironmentVariable("SUBJECT_GETBYID_URL", subjectByIdUrl);

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
            var repository = new SubjectAutomateRepository(httpClient, mapperMock.Object);

            var result = await repository.GetSubjectByIdAsync(subjectId);

            Assert.Null(result);
            mapperMock.Verify(m => m.Map<Subject>(It.IsAny<SubjectAutomateDto>()), Times.Never);
        }
    }

}