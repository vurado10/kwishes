using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Wishes.Requests;

public class GetWishById
{
    public record Request(Guid Id) : IResultRequest<Wish>;

    public class Handler : RepositoryRequestHandler<Request, Wish>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Wish>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var wish = await context.Wishes
                .FirstOrDefaultAsync(wish => wish.Id == request.Id, cancellationToken);
            if (wish is null)
                return Result.Error<Wish>($"No Wish with id: {request.Id}");
        
            await context.Entry(wish).Reference(w => w.Creator).LoadAsync(cancellationToken);
            return Result.Ok(wish);
        }
    }
}