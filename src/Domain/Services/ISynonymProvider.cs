namespace LimbooCards.Domain.Services
{
    public interface ISynonymProvider
    {
        public Task<IEnumerable<string>> GetSynonymsAsync(string word);
    }
}