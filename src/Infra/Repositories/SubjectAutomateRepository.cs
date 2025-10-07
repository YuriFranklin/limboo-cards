namespace LimbooCards.Infra.Repositories
{
    using System.Net.Http.Json;
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using LimbooCards.Infra.DTOs;
    using LimbooCards.Infra.Settings;
    using Microsoft.Extensions.Options;

    public class SubjectAutomateRepository(HttpClient httpClient, IMapper mapper, IOptions<SubjectSettings> options) : ISubjectRepository
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IMapper mapper = mapper;
        private readonly SubjectSettings settings = options.Value;

        public Task AddSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSubjectAsync(Guid subjectId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            var result = await httpClient.GetFromJsonAsync<SubjectAutomateDto[]>(settings.GetAllUrl);
            return result?.Select(dto => mapper.Map<Subject>(dto)) ?? [];
        }

        public async Task<Subject?> GetSubjectByIdAsync(Guid subjectId)
        {
            var url = $"{settings.GetByIdUrl}&subject-id={subjectId}";
            var result = await httpClient.GetFromJsonAsync<SubjectAutomateDto>(url);
            return result is null ? null : mapper.Map<Subject>(result);
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsPageAsync(Guid? afterId, int pageSize)
        {
            var url = $"{settings.GetPagedUrl}&after={afterId}&first={pageSize}";
            var result = await httpClient.GetFromJsonAsync<SubjectsPageAutomateResponseDto>(url);

            return result?.Items?.Select(dto => mapper.Map<Subject>(dto)).ToList()
                   ?? [];
        }
    }
}
