namespace LimbooCards.Presentation.GraphQL.Mutations
{
    using LimbooCards.Application.Services;
    using AutoMapper;
    using LimbooCards.Presentation.GraphQL.Models;

    public class SubjectMutations(SubjectApplicationService subjectService, IMapper mapper)
    {
        private readonly SubjectApplicationService _subjectService = subjectService;
        private readonly IMapper _mapper = mapper;

        public async Task<CardModel?> EnsureCardForSubject(string plannerId, Guid subjectId)
        {
            var card = await _subjectService.EnsureCardForSubject(plannerId, subjectId);
            if (card == null) return null;
            return _mapper.Map<CardModel>(card);
        }
    }
}