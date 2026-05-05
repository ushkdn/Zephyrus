using System.Text.Json;
using StackExchange.Redis;
using Zephyrus.Identity.Infrastructure.Settings;
using Zephyrus.SharedKernel.Common.Cache;

namespace Zephyrus.Identity.Infrastructure.Services;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : ICacheService
{
    private readonly IDatabase redis = connectionMultiplexer.GetDatabase();

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration, CancellationToken cancellationToken)
    {
        var jsonValue = JsonSerializer.Serialize(value);
        await redis.StringSetAsync(key, jsonValue, expiration,  When.Always);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var value = await redis.StringGetAsync(key);

        if (!value.HasValue) return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await redis.KeyDeleteAsync(key);
    }
}