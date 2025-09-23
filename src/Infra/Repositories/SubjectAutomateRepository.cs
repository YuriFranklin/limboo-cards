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

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            var url = Environment.GetEnvironmentVariable("SUBJECT_GETALL_URL")
                ?? throw new InvalidOperationException("SUBJECT_GETALL_URL not set.");

            var result = await httpClient.GetFromJsonAsync<SubjectAutomateDto[]>(url);

            return result?.Select(dto => mapper.Map<Subject>(dto))
                   ?? [];
        }

        public async Task<Subject?> GetSubjectByIdAsync(Guid subjectId)
        {
            var url = Environment.GetEnvironmentVariable("SUBJECT_GETBYID_URL")
            ?? throw new InvalidOperationException("SUBJECT_GETBYID_URL not set.");

            var result = await this.httpClient.GetFromJsonAsync<SubjectAutomateDto>($"{url}&subject-id={subjectId}");

            if (result == null) { return null; }

            var subject = mapper.Map<Subject>(result);

            return subject;
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsPageAsync(Guid? afterId, int pageSize)
        {
            var url = Environment.GetEnvironmentVariable("SUBJECT_GETPAGED_URL")
                ?? throw new InvalidOperationException("SUBJECT_GETPAGED_URL not set.");

            var result = await httpClient.GetFromJsonAsync<SubjectsPageAutomateResponseDto>(
                $"{url}&after={afterId}&first={pageSize}");

            var subjects = result?.Items?
                .Select(dto => mapper.Map<Subject>(dto))
                .ToList()
                ?? new List<Subject>();

            return subjects;
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
