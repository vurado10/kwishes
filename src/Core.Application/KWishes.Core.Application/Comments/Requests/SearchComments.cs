using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Comments.Requests;

public static class SearchComments
{
    public record Request(
        WishId WishId,
        DateTime From,
        DateTime To,
        int Skip,
        int Take
    ) : IResultRequest<IReadOnlyList<Comment>>;

    public class Handler : RepositoryRequestHandler<Request, IReadOnlyList<Comment>>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<IReadOnlyList<Comment>>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var comments = await context.Comments
                .AsNoTracking()
                .Where(comment => 
                    comment.IsDeleted == false
                    && comment.WishId == request.WishId
                    && comment.CreatedAt >= request.From 
                    && comment.CreatedAt < request.To)
                .OrderBy(comment => comment.CreatedAt)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync(cancellationToken);

            return Result.Ok<IReadOnlyList<Comment>>(comments);
        }
    }
}