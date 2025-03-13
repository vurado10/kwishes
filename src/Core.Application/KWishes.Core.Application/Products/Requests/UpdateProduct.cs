using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Products.Requests;

public class UpdateProduct
{
    public record Request(Product product) : IResultRequest<Product>;

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
            var product =  await context.Products.FirstOrDefaultAsync(
                p => p.Id == request.product.Id, cancellationToken);
            if (product is null)
            {
                return Result.Error<Product>($"No product with id: {request.product.Id}");
            }

            product.Logo = request.product.Logo;
            product.Name = request.product.Name;
            
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(product);
        }
    }
}