using System.Text;
using System.Text.Json;
using NATS.Client;
using NATS.Client.KeyValue;
using NATS.Client.JetStream;
using Microsoft.Extensions.Logging;
using LimbooCards.Application.Ports;
using NATS.Client.Internals;

namespace LimbooCards.Infra.KeyValues
{
    public class NatsKeyValueStore(IConnection connection, ILogger<NatsKeyValueStore> logger) : IKeyValueStore
    {
        private readonly IConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        private readonly ILogger<NatsKeyValueStore> _logger = logger;

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            IncludeFields = true,
        };

        private IKeyValue GetOrCreateBucket(string bucket, TimeSpan? ttl = null)
        {
            try
            {
                return _connection.CreateKeyValueContext(bucket);
            }
            catch (NATSJetStreamException)
            {
                try
                {
                    var kvm = _connection.CreateKeyValueManagementContext();
                    var builder = KeyValueConfiguration.Builder().WithName(bucket);

                    var effectiveTtl = ttl ?? TimeSpan.FromHours(1);
                    builder.WithTtl(Duration.OfMillis((long)effectiveTtl.TotalMilliseconds));

                    var cfg = builder.Build();
                    kvm.Create(cfg);

                    _logger.LogInformation("Bucket {Bucket} criado com TTL de {TTL}.", bucket, effectiveTtl);
                    return _connection.CreateKeyValueContext(bucket);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar/acessar bucket {Bucket}", bucket);
                    throw;
                }
            }
        }

        public async Task PutAsync<T>(string bucket, string key, T value, TimeSpan? ttl = null, CancellationToken ct = default)
        {
            var kv = GetOrCreateBucket(bucket, ttl);
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));

            await Task.Run(() =>
            {
                kv.Put(key, bytes);
            }, ct);
        }

        public async Task<T?> GetAsync<T>(string bucket, string key, CancellationToken ct = default)
        {
            var kv = GetOrCreateBucket(bucket);
            return await Task.Run(() =>
            {
                var entry = kv.Get(key);
                if (entry?.Value == null) return default;
                return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(entry.Value), _serializerOptions);
            }, ct);
        }

        public async Task DeleteAsync(string bucket, string key, CancellationToken ct = default)
        {
            var kv = GetOrCreateBucket(bucket);
            await Task.Run(() => kv.Delete(key), ct);
        }
    }
}
