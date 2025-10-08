namespace LimbooCards.UnitTests.Application
{
    public class SubjectApplicationServiceTests
    {
        private readonly Mock<ISubjectRepository> subjectRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPlannerRepository> plannerRepositoryMock = new();
        private readonly Mock<ICardRepository> cardRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        private readonly SubjectApplicationService service;

        public SubjectApplicationServiceTests()
        {
            service = new SubjectApplicationService(subjectRepositoryMock.Object, userRepositoryMock.Object, plannerRepositoryMock.Object, cardRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateSubjectAsync_Should_Create_SubjectDto()
        {
            var dto = new CreateSubjectDto
            {
                Name = "Math",
                Semester = "2025.1",
                Status = SubjectStatus.Complete,
                OwnerId = Guid.CreateVersion7().ToString(),
                CoOwnerIds = new List<string> { Guid.CreateVersion7().ToString() },
                Oferts = new List<OfertDto> { new() { Project = "P1", Module = "M1" } },
            };

            var owner = new User(dto.OwnerId, "Owner Name", "user@test.com");
            var coOwner = new User(dto.CoOwnerIds.First(), "CoOwner Name", "user@test.com");

            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.OwnerId)).ReturnsAsync(owner);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.CoOwnerIds.First())).ReturnsAsync(coOwner);

            mapperMock.Setup(m => m.Map<Ofert>(It.IsAny<OfertDto>())).Returns((OfertDto o) => new Ofert(o.Project, o.Module));

            var subjectDto = new SubjectDto
            {
                Name = dto.Name,
                Semester = dto.Semester,
                Status = dto.Status,
                Owner = new UserDto { Id = owner.Id, FullName = owner.FullName },
                CoOwners = new List<UserDto> { new UserDto { Id = coOwner.Id, FullName = coOwner.FullName } },
                Oferts = dto.Oferts.Select(o => new OfertDto { Project = o.Project, Module = o.Module }).ToList()
            };

            mapperMock.Setup(m => m.Map<SubjectDto>(It.IsAny<Subject>())).Returns(subjectDto);

            var result = await service.CreateSubjectAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(owner.FullName, result.Owner?.FullName);
            Assert.NotNull(result.CoOwners);
            Assert.Contains(result.CoOwners, c => c.Id == coOwner.Id && c.FullName == coOwner.FullName);
            subjectRepositoryMock.Verify(r => r.AddSubjectAsync(It.IsAny<Subject>()), Times.Once);
        }


        [Fact]
        public async Task GetSubjectByIdAsync_Should_Return_SubjectDto_When_Found()
        {
            var subjectId = Guid.CreateVersion7();
            var subject = new Subject(subjectId, null, "Math", "2025.1", SubjectStatus.Complete, new List<Ofert> { new("P1", "M1") });
            var dto = new SubjectDto { Name = "Math" };

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync(subject);
            mapperMock.Setup(m => m.Map<SubjectDto>(subject)).Returns(dto);

            var result = await service.GetSubjectByIdAsync(subjectId);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
        }

        [Fact]
        public async Task GetAllSubjectsAsync_Should_Return_SubjectDtos()
        {
            var subjects = new List<Subject>
        {
            new(Guid.CreateVersion7(), null, "Math", "2025.1", SubjectStatus.Complete, new List<Ofert> { new("P1","M1") }),
            new(Guid.CreateVersion7(), null, "Physics", "2025.1", SubjectStatus.Complete, new List<Ofert> { new("P2","M2") })
        };

            var dtos = subjects.Select(s => new SubjectDto { Name = s.Name }).ToList();

            subjectRepositoryMock.Setup(r => r.GetAllSubjectsAsync()).ReturnsAsync(subjects);
            mapperMock.Setup(m => m.Map<IEnumerable<SubjectDto>>(subjects)).Returns(dtos);

            var result = await service.GetAllSubjectsAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Math");
            Assert.Contains(result, r => r.Name == "Physics");
        }

        [Fact]
        public async Task UpdateSubjectAsync_Should_Update_Subject()
        {
            var dto = new UpdateSubjectDto
            {
                Id = Guid.CreateVersion7(),
                Name = "Math Updated",
                Semester = "2025.1",
                Status = SubjectStatus.Complete,
                OwnerId = Guid.CreateVersion7().ToString(),
                CoOwnerIds = new List<string> { Guid.CreateVersion7().ToString() },
                Oferts = new List<OfertDto> { new() { Project = "P1", Module = "M1" } }
            };

            var existingSubject = new Subject(dto.Id, null, "Math", "2025.1", SubjectStatus.Complete, new List<Ofert> { new("P0", "M0") });
            var owner = new User(dto.OwnerId, "Owner Name", "user@test.com");
            var coOwner = new User(dto.CoOwnerIds.First(), "CoOwner Name", "user@test.com");

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(dto.Id)).ReturnsAsync(existingSubject);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.OwnerId)).ReturnsAsync(owner);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.CoOwnerIds.First())).ReturnsAsync(coOwner);
            mapperMock.Setup(m => m.Map<Ofert>(It.IsAny<OfertDto>())).Returns((OfertDto o) => new Ofert(o.Project, o.Module));

            await service.UpdateSubjectAsync(dto);

            subjectRepositoryMock.Verify(r => r.UpdateSubjectAsync(It.IsAny<Subject>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSubjectAsync_Should_Call_Repository()
        {
            var subjectId = Guid.CreateVersion7();

            await service.DeleteSubjectAsync(subjectId);

            subjectRepositoryMock.Verify(r => r.DeleteSubjectAsync(subjectId), Times.Once);
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldReturnExistingCard_WhenMatchIsFound()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var plannerId = "planner1";

            var subject = new Subject(subjectId, null, "Matemática I", "2025.1", SubjectStatus.Complete, [new Ofert("DIG", "AE")]);
            var existingCard = new Card("CARD: Matemática 1", false, "user", plannerId, id: "card123");

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync(subject);
            cardRepositoryMock.Setup(r => r.GetAllCardsAsync()).ReturnsAsync(new List<Card> { existingCard });

            // Act
            var result = await service.EnsureCardForSubject(plannerId, subjectId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingCard.Id);
            cardRepositoryMock.Verify(r => r.AddCardAsync(It.IsAny<Card>()), Times.Never);
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldCreateAndReturnNewCard_WhenNoMatchFoundAndSubjectIsEligible()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var plannerId = "planner1";

            var contents = new List<Content> { new("Topic 1", "Item A", ContentStatus.Missing) };
            var subject = new Subject(subjectId, "123", "História", "2025.1", SubjectStatus.Complete, [new Ofert("DIG", "AE")], contents: contents, owner: new User("123", "Test User", "test"));
            var planner = new Planner(plannerId, "Main Planner", new List<PlannerBucket> { new("b1", "Default", true, true, true) });

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync(subject);
            cardRepositoryMock.Setup(r => r.GetAllCardsAsync()).ReturnsAsync(new List<Card>());
            plannerRepositoryMock.Setup(r => r.GetPlannerByIdAsync(plannerId)).ReturnsAsync(planner);

            // Act
            var result = await service.EnsureCardForSubject(plannerId, subjectId);

            // Assert
            result.Should().NotBeNull();
            result.PlanId.Should().Be(plannerId);
            cardRepositoryMock.Verify(r => r.AddCardAsync(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldThrowArgumentException_WhenSubjectNotFound()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync((Subject?)null);

            // Act
            Func<Task> act = async () => await service.EnsureCardForSubject("planner1", subjectId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"*Subject with Id {subjectId} not found*");
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldThrowArgumentException_WhenPlannerNotFound()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var plannerId = "planner-nao-existe";

            var subject = new Subject(subjectId, null, "Matemática", "2025.1", SubjectStatus.Complete, [new Ofert("DIG", "AE")], contents: new List<Content>());

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync(subject);
            cardRepositoryMock.Setup(r => r.GetAllCardsAsync()).ReturnsAsync(new List<Card>());
            plannerRepositoryMock.Setup(r => r.GetPlannerByIdAsync(plannerId)).ReturnsAsync((Planner?)null);

            // Act
            Func<Task> act = async () => await service.EnsureCardForSubject(plannerId, subjectId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"*Planner with Id {plannerId} not found*");
        }

        [Fact]
        public async Task EnsureCardForSubject_ShouldThrowInvalidOperationException_WhenCardCannotBeCreated()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var plannerId = "planner1";

            var subject = new Subject(subjectId, null, "História", "2025.1", SubjectStatus.Complete, [new Ofert("DIG", "AE")], contents: new List<Content>());
            var planner = new Planner(plannerId, "Main Planner", new List<PlannerBucket> { new("b1", "Default", true, true, true) });

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(subjectId)).ReturnsAsync(subject);
            cardRepositoryMock.Setup(r => r.GetAllCardsAsync()).ReturnsAsync(new List<Card>());
            plannerRepositoryMock.Setup(r => r.GetPlannerByIdAsync(plannerId)).ReturnsAsync(planner);

            // Act
            Func<Task> act = async () => await service.EnsureCardForSubject(plannerId, subjectId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"*Cannot create a card from Subject '{subject.Name}'*");
        }
    }

}
