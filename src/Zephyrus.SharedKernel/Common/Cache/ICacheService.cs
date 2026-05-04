namespace Zephyrus.SharedKernel.Common.Cache;

public interface ICacheService
{
    Task SetAsync(string key, string value, TimeSpan? expiration, CancellationToken cancellationToken);

    Task<string?> GetAsync(string key, CancellationToken cancellationToken);

    Task RemoveAsync(string key, CancellationToken cancellationToken);
}