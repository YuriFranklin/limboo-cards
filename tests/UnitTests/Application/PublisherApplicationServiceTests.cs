namespace LimbooCards.UnitTests.Application
{
    public class PublisherApplicationServiceTests
    {
        private readonly Mock<IPublisherRepository> publisherRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        private readonly PublisherApplicationService service;

        public PublisherApplicationServiceTests()
        {
            service = new PublisherApplicationService(publisherRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreatePublisherAsync_Should_Create_And_Return_PublisherDto()
        {
            var dto = new CreatePublisherDto { Name = "Packt" };
            var publisher = new Publisher(Guid.CreateVersion7(), dto.Name);
            var publisherDto = new PublisherDto { Id = publisher.Id, Name = publisher.Name };

            mapperMock.Setup(m => m.Map<Publisher>(dto)).Returns(publisher);
            mapperMock.Setup(m => m.Map<PublisherDto>(publisher)).Returns(publisherDto);

            var result = await service.CreatePublisherAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            publisherRepositoryMock.Verify(r => r.AddPublisherAsync(publisher), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_Should_Return_PublisherDto_When_Found()
        {
            var publisherId = Guid.CreateVersion7();
            var publisher = new Publisher(publisherId, "Manning");
            var publisherDto = new PublisherDto { Id = publisherId, Name = publisher.Name };

            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(publisherId)).ReturnsAsync(publisher);
            mapperMock.Setup(m => m.Map<PublisherDto>(publisher)).Returns(publisherDto);

            var result = await service.GetPublisherByIdAsync(publisherId);

            Assert.NotNull(result);
            Assert.Equal(publisher.Name, result.Name);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_Should_Return_Null_When_NotFound()
        {
            var publisherId = Guid.CreateVersion7();

            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(publisherId))
                .ReturnsAsync((Publisher?)null);

            var result = await service.GetPublisherByIdAsync(publisherId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPublisherByNameAsync_Should_Return_PublisherDto_When_Found()
        {
            var publisher = new Publisher(Guid.CreateVersion7(), "O’Reilly");
            var publisherDto = new PublisherDto { Id = publisher.Id, Name = publisher.Name };

            publisherRepositoryMock.Setup(r => r.GetPublisherByNameAsync(publisher.Name)).ReturnsAsync(publisher);
            mapperMock.Setup(m => m.Map<PublisherDto>(publisher)).Returns(publisherDto);

            var result = await service.GetPublisherByNameAsync(publisher.Name);

            Assert.NotNull(result);
            Assert.Equal(publisher.Name, result.Name);
        }

        [Fact]
        public async Task GetPublisherByNameAsync_Should_Return_Null_When_NotFound()
        {
            publisherRepositoryMock.Setup(r => r.GetPublisherByNameAsync("Unknown"))
                .ReturnsAsync((Publisher?)null);

            var result = await service.GetPublisherByNameAsync("Unknown");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPublishersAsync_Should_Return_List_Of_PublisherDto()
        {
            var publishers = new List<Publisher>
            {
                new Publisher(Guid.CreateVersion7(), "Springer"),
                new Publisher(Guid.CreateVersion7(), "Elsevier")
            };
            var publisherDtos = publishers.Select(p => new PublisherDto { Id = p.Id, Name = p.Name }).ToList();

            publisherRepositoryMock.Setup(r => r.GetAllPublishersAsync()).ReturnsAsync(publishers);
            mapperMock.Setup(m => m.Map<IEnumerable<PublisherDto>>(publishers)).Returns(publisherDtos);

            var result = await service.GetAllPublishersAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Springer");
            Assert.Contains(result, r => r.Name == "Elsevier");
        }

        [Fact]
        public async Task UpdatePublisherAsync_Should_Call_Repository()
        {
            var dto = new UpdatePublisherDto { Id = Guid.CreateVersion7(), Name = "Updated Publisher" };
            var publisher = new Publisher(dto.Id, dto.Name);

            mapperMock.Setup(m => m.Map<Publisher>(dto)).Returns(publisher);

            await service.UpdatePublisherAsync(dto);

            publisherRepositoryMock.Verify(r => r.UpdatePublisherAsync(publisher), Times.Once);
        }

        [Fact]
        public async Task DeletePublisherAsync_Should_Call_Repository()
        {
            var publisherId = Guid.CreateVersion7();

            await service.DeletePublisherAsync(publisherId);

            publisherRepositoryMock.Verify(r => r.DeletePublisherAsync(publisherId), Times.Once);
        }
    }
}
