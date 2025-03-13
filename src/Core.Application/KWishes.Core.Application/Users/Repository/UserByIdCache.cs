using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Caches;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Repository;

public class UserByIdCache : AbsoluteLruCache<UserId, User>
{
    private readonly IDbContextFactory<RepositoryContext> dbContextFactory;

    public UserByIdCache(
        IDbContextFactory<RepositoryContext> dbContextFactory,
        AbsoluteLruCacheOptions options) : base(options)
    {
        this.dbContextFactory = dbContextFactory;
    }

    protected override async ValueTask<User?> GetItemByKey(UserId key, CancellationToken cancellationToken)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Users.FirstOrDefaultAsync(u => u.Id == key, cancellationToken);
    }
}