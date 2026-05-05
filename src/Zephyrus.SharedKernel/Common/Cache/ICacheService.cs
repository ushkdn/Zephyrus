namespace Zephyrus.SharedKernel.Common.Cache;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiration, CancellationToken cancellationToken);

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

    Task RemoveAsync(string key, CancellationToken cancellationToken);
}