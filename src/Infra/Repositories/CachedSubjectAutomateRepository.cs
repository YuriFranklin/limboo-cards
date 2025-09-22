namespace LimbooCards.Infra.Repositories
{
    using LimbooCards.Application.Ports;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;
    using System;

    public class CachedSubjectAutomateRepository(ISubjectRepository inner, IKeyValueStore cache) : ISubjectRepository
    {
        private readonly ISubjectRepository _inner = inner;
        private readonly IKeyValueStore _cache = cache;
        private readonly string _bucket = "subjects";
        private readonly TimeSpan _ttl = TimeSpan.FromHours(1);

        public async Task<Subject?> GetSubjectByIdAsync(Guid subjectId)
        {
            Console.WriteLine("AAAAAAA");
            var cached = await _cache.GetAsync<Subject>(_bucket, subjectId.ToString());
            if (cached != null) return cached;

            var subject = await _inner.GetSubjectByIdAsync(subjectId);
            if (subject != null)
                _ = _cache.PutAsync(_bucket, subjectId.ToString(), subject, _ttl);

            Console.WriteLine(subject?.ToString());

            return subject;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            var cachedList = await _cache.GetAsync<List<Subject>>(_bucket, "all");
            if (cachedList != null)
                return cachedList;

            var subjects = await _inner.GetAllSubjectsAsync();

            foreach (var subject in subjects)
                _ = _cache.PutAsync(_bucket, subject.Id.ToString(), subject, _ttl);

            _ = _cache.PutAsync(_bucket, "all", subjects, _ttl);

            return subjects;
        }

        public Task AddSubjectAsync(Subject subject)
        {
            _ = _cache.PutAsync(_bucket, subject.Id.ToString(), subject, _ttl);
            _ = _cache.DeleteAsync(_bucket, "all");
            return _inner.AddSubjectAsync(subject);
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            _ = _cache.PutAsync(_bucket, subject.Id.ToString(), subject, _ttl);

            _ = Task.Run(async () =>
            {
                var all = await _cache.GetAsync<List<Subject>>(_bucket, "all");
                if (all != null)
                {
                    all.RemoveAll(s => s.Id == subject.Id);
                    all.Add(subject);
                    _ = _cache.PutAsync(_bucket, "all", all, _ttl);
                }
            });

            return _inner.UpdateSubjectAsync(subject);
        }

        public Task DeleteSubjectAsync(Guid subjectId)
        {
            _ = _cache.DeleteAsync(_bucket, subjectId.ToString());
            _ = _cache.DeleteAsync(_bucket, "all");
            return _inner.DeleteSubjectAsync(subjectId);
        }
    }
}
