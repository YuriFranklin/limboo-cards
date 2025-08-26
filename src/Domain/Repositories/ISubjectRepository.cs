namespace LimbooCards.Domain.Repositories
{
    using LimbooCards.Domain.Entities;
    public interface ISubjectRepository
    {
        public Task<Subject?> GetSubjectByIdAsync(Guid subjectId);
        public Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        public Task AddSubjectAsync(Subject subject);
        public Task UpdateSubjectAsync(Subject subject);
        public Task DeleteSubjectAsync(Guid subjectId);
    }
}