namespace LimbooCards.UnitTests.Infra
{
    public class CardAutomateRepositoryTests
    {
        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnMappedCard_WhenDtoExists()
        {
            var cardId = Guid.CreateVersion7();
            var planId = Guid.CreateVersion7();
            var bucketId = Guid.CreateVersion7();
            var createdById = Guid.CreateVersion7();

            var dto = new CardAutomateDto
            {
                id = cardId,
                planId = planId,
                bucketId = bucketId,
                title = "Matemática - Exercícios",
                description = "Lista de exercícios de equações",
                hasDescription = true,
                subjectId = Guid.CreateVersion7(),
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
                        req.RequestUri!.ToString().EndsWith($"/cards/{cardId}")),
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

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CardMappingProfile>();
                cfg.AddProfile<ChecklistItemMappingProfile>();
            });
            var mapper = configuration.CreateMapper();

            var repository = new CardAutomateRepository(httpClient, mapper);

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
            var cardId = Guid.CreateVersion7();

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

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var mapperMock = new Mock<IMapper>();
            var repository = new CardAutomateRepository(httpClient, mapperMock.Object);

            // Act
            var result = await repository.GetCardByIdAsync(cardId);

            // Assert
            result.Should().BeNull();
            mapperMock.Verify(m => m.Map<Card>(It.IsAny<CardAutomateDto>()), Times.Never);
        }
    }
}
