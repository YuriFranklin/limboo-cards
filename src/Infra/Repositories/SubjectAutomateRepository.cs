namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.DTOs;
    using LimbooCards.Infra.Mappings;

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

        public Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Subject?> GetSubjectByIdAsync(Guid subjectId)
        {
            var result = await this.httpClient.GetFromJsonAsync<SubjectAutomateDto>($"/subjects/{subjectId}");

            if (result == null) { return null; }

            return mapper.Map<Subject>(result);
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
