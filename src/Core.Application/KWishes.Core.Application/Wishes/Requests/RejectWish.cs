using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Caches;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Application.Users.Repository;
using KWishes.Core.Application.Users.Requests;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Wishes.Requests;

public static class RejectWish
{
    public record Request(WishId WishId, UserId UserId, string RejectText) : IResultRequest<Nothing>;

    public class Handler : RepositoryRequestHandler<Request, Nothing>
    {
        private readonly IReadOnlyCache<UserId, User> userById;

        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory, IReadOnlyCache<UserId, User> userById) 
            : base(dbContextFactory)
        {
            this.userById = userById;
        }

        protected override async ValueTask<Result<Nothing>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await userById.Get(request.UserId, cancellationToken);
            if (user is null)
                return Result.Error<Nothing>(UserErrorRegistry.NoWithId(request.UserId));
            
            var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var count = await context.Wishes
                    .Where(w => w.Id == request.WishId)
                    .ExecuteUpdateAsync(
                        calls => calls.SetProperty(w => w.Status, WishStatus.Rejected),
                        cancellationToken);

                if (count <= 0)
                    return Result.Error<Nothing>("No wishes");

                context.Comments.Create(request.WishId, user, request.RejectText);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Error<Nothing>("Reject fails");
            }
            
            await transaction.CommitAsync(cancellationToken);

            return Result.Ok();
        }
    }
}