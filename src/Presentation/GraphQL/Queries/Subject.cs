namespace LimbooCards.Presentation.GraphQL.Queries
{
    using AutoMapper;
    using LimbooCards.Application.Services;
    using LimbooCards.Presentation.GraphQL.Models;

    public class SubjectQueries(SubjectApplicationService subjectService, IMapper mapper)
    {
        private readonly SubjectApplicationService _subjectService = subjectService;
        private readonly IMapper _mapper = mapper;

        public async Task<SubjectModel?> GetSubjectByIdAsync(Guid id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null) return null;

            return _mapper.Map<SubjectModel>(subject);
        }
    }
}