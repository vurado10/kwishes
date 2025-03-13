using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Wishes.Requests;

public static class UpdateWish
{
    public record Request(WishId WishId, string Text) : IResultRequest<Nothing>;

    public class Handler : RepositoryRequestHandler<Request, Nothing>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Nothing>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var count = await context.Wishes
                .Where(w => w.Id == request.WishId)
                .ExecuteUpdateAsync(
                    calls => calls
                        .SetProperty(w => w.Text, request.Text)
                        .SetProperty(w => w.LastUpdateAt, DateTime.UtcNow),
                    cancellationToken);

            return count <= 0 
                ? Result.Error<Nothing>($"Wish doesn't exist. Id: {request.WishId}") 
                : Result.Ok();
        }
    }
}