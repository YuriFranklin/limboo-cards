namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.DTOs;

    public class SubjectAutomateRepository(HttpClient httpClient, IMapper mapper) : ISubjectRepository
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IMapper mapper = mapper;
        public Task AddSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSubjectAsync(Guid subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Subject>> GetAllSubjectsAsync(Guid? afterId, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Subject?> GetSubjectByIdAsync(Guid subjectId)
        {
            var url = Environment.GetEnvironmentVariable("SUBJECT_GETBYID_URL")
            ?? throw new InvalidOperationException("SUBJECT_GETALL_URL not set.");

            var result = await this.httpClient.GetFromJsonAsync<SubjectAutomateDto>($"{url}&subject-id={subjectId}");

            if (result == null) { return null; }

            var subject = mapper.Map<Subject>(result);

            return subject;
        }

        public Task<IReadOnlyList<Subject>> GetSubjectsPageAsync(Guid? afterId, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
