using KWishes.Core.Domain.Products;

namespace KWishes.API.Dto.Products;

public static class DtoProductMapper
{
    public static DtoProduct MapFrom(Product product)
    {
        return new DtoProduct(
            product.Id,
            product.Name,
            product.Logo);
    }
}