namespace LimbooCards.Presentation.GraphQL.Queries
{
    using AutoMapper;
    using LimbooCards.Application.Services;
    using LimbooCards.Presentation.GraphQL.Models;

    public class SubjectQueries(SubjectApplicationService subjectService, IMapper mapper)
    {
        private readonly SubjectApplicationService _subjectService = subjectService;
        private readonly IMapper _mapper = mapper;

        public async Task<SubjectModel?> GetSubjectAsync(Guid id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null) return null;

            return _mapper.Map<SubjectModel>(subject);
        }

        public async Task<SubjectConnection> GetSubjectsAsync(int? first = null, string? after = null)
        {
            var result = await _subjectService.GetSubjectsPagedAsync(first, after);

            var edges = result.Items.Select(s => new SubjectEdge
            {
                Node = _mapper.Map<SubjectModel>(s),
                Cursor = EncodeCursor(s.Id)
            }).ToList();

            var pageInfo = new PageInfo
            {
                StartCursor = edges.FirstOrDefault()?.Cursor,
                EndCursor = edges.LastOrDefault()?.Cursor,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            };

            return new SubjectConnection
            {
                Edges = edges,
                PageInfo = pageInfo
            };
        }

        private static string EncodeCursor(Guid id)
        {
            var plainText = id.ToString();
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }
    }
}