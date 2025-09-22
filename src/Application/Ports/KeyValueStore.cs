namespace LimbooCards.Application.Ports
{
    public interface IKeyValueStore
    {
        public Task PutAsync<T>(string bucket, string key, T value, TimeSpan? ttl = null, CancellationToken ct = default);

        public Task<T?> GetAsync<T>(string bucket, string key, CancellationToken ct = default);

        public Task DeleteAsync(string bucket, string key, CancellationToken ct = default);
    }
}
