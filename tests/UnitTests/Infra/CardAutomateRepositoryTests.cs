namespace LimbooCards.UnitTests.Infra
{
    public class CardAutomateRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly IOptions<CardSettings> _options;
        public CardAutomateRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CardMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
            });

            _mapper = config.CreateMapper();

            var settings = new CardSettings
            {
                GetByIdUrl = "http://localhost/cards?api-version=1",
                GetAllUrl = "http://localhost/cards/all?api-version=1"
            };

            _options = Options.Create(settings);
        }


        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnMappedCard_WhenDtoExists()
        {
            var cardByIdUrl = "http://localhost/cards?api-version=1";
            Environment.SetEnvironmentVariable("CARD_GETBYID_URL", cardByIdUrl);

            var cardId = Guid.CreateVersion7().ToString();
            var planId = Guid.CreateVersion7().ToString();
            var bucketId = Guid.CreateVersion7().ToString();
            var createdById = Guid.CreateVersion7().ToString();

            var dto = new CardAutomateDto
            {
                id = cardId,
                planId = planId,
                bucketId = bucketId,
                title = "Matemática - Exercícios",
                description = "Lista de exercícios de equações",
                hasDescription = true,
                subjectId = Guid.CreateVersion7().ToString(),
                orderHint = "A123",
                createdDateTime = DateTime.UtcNow.AddDays(-1).ToString("O"),
                dueDateTime = DateTime.UtcNow.AddDays(3).ToString("O"),
                createdBy = new CreatedBy
                {
                    user = new CreatedByUser { id = createdById }
                },
                appliedCategories = new Dictionary<string, bool>
                {
                    { "Álgebra", true }
                },
                checklist = new List<ChecklistItemAutomateDto>
                {
                    new ChecklistItemAutomateDto
                    {
                        id = "chk1",
                        value = new ChecklistValue
                        {
                            title = "Resolver 5 equações",
                            isChecked = false,
                            orderHint = "B123",
                            lastModifiedDateTime = DateTime.UtcNow,
                            lastModifiedBy = new LastModifiedBy
                            {
                                user = new LastModifiedUser
                                {
                                    id = Guid.CreateVersion7(),
                                    displayName = "Professor"
                                }
                            }
                        }
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
                    req.RequestUri!.ToString().StartsWith(cardByIdUrl)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(dto)
            });

            var httpClient = new HttpClient(handlerMock.Object);

            var repository = new CardAutomateRepository(httpClient, _mapper, _options);

            var result = await repository.GetCardByIdAsync(cardId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(cardId);
            result.PlanId.Should().Be(planId);
            result.BucketId.Should().Be(bucketId);
            result.Title.Should().Be("Matemática - Exercícios");
            result.Description.Should().Be("Lista de exercícios de equações");
            result.CreatedBy.Should().Be(createdById);
            result.AppliedCategories.Should().ContainKey("Álgebra");
            result.Checklist.Should().HaveCount(1);
            result.Checklist![0].Title.Should().Be("Resolver 5 equações");
        }

        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnNull_WhenApiReturnsNull()
        {
            // Arrange
            var cardByIdUrl = "http://localhost/cards?api-version=1";
            Environment.SetEnvironmentVariable("CARD_GETBYID_URL", cardByIdUrl);

            var cardId = Guid.CreateVersion7().ToString();

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
                    Content = JsonContent.Create<CardAutomateDto?>(null)
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var mapperMock = new Mock<IMapper>();
            var repository = new CardAutomateRepository(httpClient, mapperMock.Object, _options);

            // Act
            var result = await repository.GetCardByIdAsync(cardId);

            // Assert
            result.Should().BeNull();
            mapperMock.Verify(m => m.Map<Card>(It.IsAny<CardAutomateDto>()), Times.Never);
        }
    }
}
