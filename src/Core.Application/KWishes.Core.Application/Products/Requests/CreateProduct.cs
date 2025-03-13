using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Products.Requests;

public class CreateProduct
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
            if (product is not null)
            {
                return Result.Error<Product>($"Product  already exists. Id: {request.product.Id}");
            }

            var newProduct = new Product
            {
                Id = request.product.Id,
                Name = request.product.Name,
                Logo = request.product.Logo
            };
            var entry = await context.Products.AddAsync(newProduct, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(entry.Entity);
        }
    }
}