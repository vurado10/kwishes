using BitFaster.Caching.Lru;

namespace KWishes.Core.Application.Misc.Caches;

public record AbsoluteLruCacheOptions(
    int Capacity,
    TimeSpan TimeToLive
);

public abstract class AbsoluteLruCache<TKey, TValue> : IReadOnlyCache<TKey, TValue>
    where TKey : notnull
{
    private readonly ConcurrentTLru<TKey, TValue> innerCache;
    
    protected AbsoluteLruCache(AbsoluteLruCacheOptions options)
    {
        innerCache = new ConcurrentTLru<TKey, TValue>(options.Capacity, options.TimeToLive);
    }

    public async ValueTask<TValue?> Get(TKey key, CancellationToken cancellationToken = default)
    {
        if (innerCache.TryGet(key, out var value))
            return value;

        value = await GetItemByKey(key, cancellationToken);
        
        return value is null ? default : innerCache.GetOrAdd(key, _ => value);
    }

    protected abstract ValueTask<TValue?> GetItemByKey(TKey key, CancellationToken cancellationToken);
}