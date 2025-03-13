using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Products.Requests;

public class GetProducts
{
    public record Request : IResultRequest<Product[]>;

    public class Handler : RepositoryRequestHandler<Request, Product[]>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Product[]>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var products = await context.Products.ToArrayAsync(cancellationToken);
            return Result.Ok(products);
        }
    }
}