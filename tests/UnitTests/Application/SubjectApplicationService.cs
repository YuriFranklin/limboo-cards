namespace LimbooCards.UnitTests.Application
{
    public class SubjectApplicationServiceTests
    {
        private readonly Mock<ISubjectRepository> subjectRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        private readonly SubjectApplicationService service;

        public SubjectApplicationServiceTests()
        {
            service = new SubjectApplicationService(subjectRepositoryMock.Object, userRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateSubjectAsync_Should_Create_SubjectDto()
        {
            var dto = new CreateSubjectDto
            {
                Name = "Math",
                Semester = "2025.1",
                Status = SubjectStatus.Complete,
                OwnerId = Guid.CreateVersion7(),
                CoOwnerIds = new List<Guid> { Guid.CreateVersion7() },
                Oferts = new List<OfertDto> { new() { Project = "P1", Module = "M1" } },
            };

            var owner = new User(dto.OwnerId.Value, "Owner Name", "user@test.com");
            var coOwner = new User(dto.CoOwnerIds.First(), "CoOwner Name", "user@test.com");

            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.OwnerId.Value)).ReturnsAsync(owner);
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
                OwnerId = Guid.CreateVersion7(),
                CoOwnerIds = new List<Guid> { Guid.CreateVersion7() },
                Oferts = new List<OfertDto> { new() { Project = "P1", Module = "M1" } }
            };

            var existingSubject = new Subject(dto.Id, null, "Math", "2025.1", SubjectStatus.Complete, new List<Ofert> { new("P0", "M0") });
            var owner = new User(dto.OwnerId.Value, "Owner Name", "user@test.com");
            var coOwner = new User(dto.CoOwnerIds.First(), "CoOwner Name", "user@test.com");

            subjectRepositoryMock.Setup(r => r.GetSubjectByIdAsync(dto.Id)).ReturnsAsync(existingSubject);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.OwnerId.Value)).ReturnsAsync(owner);
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
    }

}
