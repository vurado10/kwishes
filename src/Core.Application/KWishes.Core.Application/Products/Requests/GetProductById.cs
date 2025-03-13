using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Products.Requests;

public class GetProductById
{
    public record Request(string Id) : IResultRequest<Product>;

    public class Handler : RepositoryRequestHandler<Request, Product>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Product>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(product => product.Id == request.Id, cancellationToken);
        
            return product is null 
                ? Result.Error<Product>($"No Product with id: {request.Id}") 
                : Result.Ok(product);
        }
    }
}