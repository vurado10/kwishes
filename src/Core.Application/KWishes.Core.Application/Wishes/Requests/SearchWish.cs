using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Wishes.Requests;

public static class SearchWish
{
    public record Request(
        string ProductId,
        UserId? CreatorId,
        IReadOnlySet<WishStatus> Statuses,
        string? Text,
        DateTime? From,
        DateTime? To,
        int Skip,
        int Take
    ) : IResultRequest<IReadOnlyList<Wish>>;

    public class Handler : RepositoryRequestHandler<Request, IReadOnlyList<Wish>>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<IReadOnlyList<Wish>>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var query = context.Wishes
                .Include(w => w.Creator)
                .AsNoTracking()
                .Where(wish => wish.IsDeleted == false && wish.ProductId == request.ProductId);
            
            if (request.CreatorId is not null) 
                query = query.Where(wish => wish.CreatorId == request.CreatorId);

            if (request.From is not null)
                query = query.Where(wish => wish.CreatedAt >= request.From);
            
            if (request.To is not null)
                query = query.Where(wish => wish.CreatedAt < request.To);

            if (request.Statuses.Count > 0)
                query = query.Where(wish => request.Statuses.Contains(wish.Status));
            
            if (request.Text is not null)
                query = query.Where(wish => EF.Functions.Like(wish.Text, $"%{request.Text}%"));

            var wishes = await query
                .OrderByDescending(wish => wish.CreatedAt)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync(cancellationToken);

            return Result.Ok<IReadOnlyList<Wish>>(wishes);
        }
    }
}