namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Domain.Services;

    public class SubjectApplicationService(ISubjectRepository subjectRepository, IUserRepository userRepository, IPlannerRepository plannerRepository, ICardRepository cardRepository, IMapper mapper)
    {
        private readonly ISubjectRepository subjectRepository = subjectRepository;
        private readonly IUserRepository userRepository = userRepository;
        private readonly IPlannerRepository plannerRepository = plannerRepository;
        private readonly ICardRepository cardRepository = cardRepository;
        private readonly IMapper mapper = mapper;

        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto)
        {
            var oferts = dto.Oferts?.Select(o => mapper.Map<Ofert>(o)).ToList() ?? new List<Ofert>();
            var contents = dto.Contents?.Select(c => mapper.Map<Content>(c)).ToList();
            var publishers = dto.Publishers?.Select(p => mapper.Map<SubjectPublisher>(p)).ToList();

            User? owner = null;
            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                owner = await userRepository.GetUserByIdAsync(dto.OwnerId)
                        ?? throw new ArgumentException("Owner not found");
            }

            List<User>? coOwners = null;
            if (dto.CoOwnerIds != null)
            {
                coOwners = new List<User>();
                foreach (var id in dto.CoOwnerIds)
                {
                    var co = await userRepository.GetUserByIdAsync(id)
                            ?? throw new ArgumentException($"CoOwner {id} not found");
                    coOwners.Add(co);
                }
            }

            var subject = new Subject(
                id: null,
                modelId: dto.ModelId,
                name: dto.Name,
                semester: dto.Semester,
                status: dto.Status,
                oferts: oferts,
                equivalencies: dto.Equivalencies,
                contents: contents,
                owner: owner,
                coOwners: coOwners,
                publishers: publishers
            );

            await this.subjectRepository.AddSubjectAsync(subject);

            return mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto?> GetSubjectByIdAsync(Guid subjectId)
        {
            var subject = await this.subjectRepository.GetSubjectByIdAsync(subjectId);
            if (subject == null) return null;

            return mapper.Map<SubjectDto>(subject);
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await this.subjectRepository.GetAllSubjectsAsync();
            return mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }

        public async Task<PagedResult<SubjectDto>> GetSubjectsPagedAsync(int? first = 20, string? after = null)
        {
            var pageSize = first.GetValueOrDefault(20);
            Guid? afterId = DecodeCursor(after);

            var page = await subjectRepository.GetSubjectsPageAsync(afterId, pageSize);

            bool hasNextPage = page.Count() > pageSize;
            var items = page.Take(pageSize).ToList();

            return new PagedResult<SubjectDto>
            {
                Items = mapper.Map<IReadOnlyList<SubjectDto>>(items),
                HasNextPage = hasNextPage,
                HasPreviousPage = afterId.HasValue
            };
        }

        public async Task UpdateSubjectAsync(UpdateSubjectDto dto)
        {
            var subject = await this.subjectRepository.GetSubjectByIdAsync(dto.Id)
              ?? throw new ArgumentException($"Subject with Id {dto.Id} not found.");

            User? owner = null;
            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                owner = await userRepository.GetUserByIdAsync(dto.OwnerId)
                         ?? throw new ArgumentException($"Owner {dto.OwnerId} not found");
            }

            List<User>? coOwners = null;
            if (dto.CoOwnerIds != null && dto.CoOwnerIds.Any())
            {
                coOwners = new List<User>();
                foreach (var id in dto.CoOwnerIds)
                {
                    var co = await userRepository.GetUserByIdAsync(id)
                             ?? throw new ArgumentException($"CoOwner {id} not found");
                    coOwners.Add(co);
                }
            }

            var oferts = dto.Oferts?.Select(o => mapper.Map<Ofert>(o)).ToList() ?? new List<Ofert>();
            var contents = dto.Contents?.Select(c => mapper.Map<Content>(c)).ToList();
            var publishers = dto.Publishers?.Select(p => mapper.Map<SubjectPublisher>(p)).ToList();

            var updatedSubject = new Subject(
                id: subject.Id,
                modelId: dto.ModelId,
                name: dto.Name,
                semester: dto.Semester,
                status: dto.Status,
                oferts: oferts,
                equivalencies: dto.Equivalencies,
                contents: contents,
                owner: owner,
                coOwners: coOwners,
                publishers: publishers
            );

            await this.subjectRepository.UpdateSubjectAsync(updatedSubject);
        }

        public async Task DeleteSubjectAsync(Guid subjectId)
        {
            await this.subjectRepository.DeleteSubjectAsync(subjectId);
        }

        public async Task<CardDto?> EnsureCardForSubject(string plannerId, Guid subjectId)
        {
            var subject = await this.subjectRepository.GetSubjectByIdAsync(subjectId)
                ?? throw new ArgumentException($"Subject with Id {subjectId} not found.");

            var cards = await this.cardRepository.GetAllCardsAsync();

            var card = CardSubjectMatcherService.MatchCardForSubject(subject, cards);

            if (card != null)
            {
                return null;
            }

            var planner = await this.plannerRepository.GetPlannerByIdAsync(plannerId)
                ?? throw new ArgumentException($"Planner with Id {plannerId} not found.");

            var newCard = SubjectCardOrchestratorService.EnsureCardForSubject(subject, planner);

            if (newCard == null)
                return null;

            await this.cardRepository.AddCardAsync(newCard);

            return mapper.Map<CardDto>(newCard);
        }
        private static Guid? DecodeCursor(string? cursor)
        {
            if (string.IsNullOrWhiteSpace(cursor)) return null;
            var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            return Guid.TryParse(decoded, out var id) ? id : null;
        }
    }
}
