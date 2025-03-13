using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Wishes.Requests;

public class CreateWish
{
    public record Request(Wish Wish) : IResultRequest<Wish>;

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
            var wish =  await context.Wishes.FirstOrDefaultAsync(
                p => p.Id == request.Wish.Id, cancellationToken);
            
            if (wish is not null)
                return Result.Error<Wish>($"Wish  already exists. Id: {request.Wish.Id}");

            var newWish = new Wish
            {
                Id = new WishId(Guid.NewGuid()),
                ProductId = request.Wish.ProductId,
                CreatorId = request.Wish.CreatorId,
                CreatedAt = DateTime.UtcNow,
                LastUpdateAt = DateTime.UtcNow,
                Text = request.Wish.Text
            };
            
            var entry = await context.Wishes.AddAsync(newWish, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(entry.Entity);
        }
    }
}