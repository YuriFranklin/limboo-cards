namespace LimbooCards.Presentation.GraphQL.Mutations
{
    using LimbooCards.Application.Services;
    using AutoMapper;
    using LimbooCards.Presentation.GraphQL.Models;

    public class SubjectMutations(SubjectApplicationService subjectService, IMapper mapper)
    {
        private readonly SubjectApplicationService _subjectService = subjectService;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CardModel>> EnsureCardsForSubjects(string plannerId, List<Guid> subjectIds)
        {
            var cards = await _subjectService.EnsureCardsForSubjects(plannerId, subjectIds);
            if (cards == null || cards.Count == 0) return [];

            return _mapper.Map<List<CardModel>>(cards);
        }
    }
}