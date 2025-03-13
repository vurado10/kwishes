namespace KWishes.Core.Application.Misc.Caches;

public interface IReadOnlyCache<in TKey, TValue>
{
    ValueTask<TValue?> Get(TKey key, CancellationToken cancellationToken = default);
}