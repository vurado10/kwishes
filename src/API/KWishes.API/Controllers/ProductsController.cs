using KWishes.API.Dto.Errors;
using KWishes.API.Dto.Misc;
using KWishes.API.Dto.Products;
using KWishes.Core.Application.Products.Requests;
using KWishes.Core.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1/products")]
public class ProductsController : Controller
{
    private readonly ISender sender;

    public ProductsController(ISender sender)
    {
        this.sender = sender;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task <ActionResult<DtoItems<DtoProduct>>> GetProducts()
    {
        var result = await sender.Send(new GetProducts.Request());
        if (result.TryGetError(out var products, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        var dtoProducts = products.Select(DtoProductMapper.MapFrom).ToArray();
        return new DtoItems<DtoProduct>(dtoProducts);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoProduct>> GetProduct(string id)
    {
        var result = await sender.Send(new GetProductById.Request(id));
        if (result.TryGetError(out var product, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return DtoProductMapper.MapFrom(product);
    }

    [HttpPost("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoProduct>> PostProduct(string id, [FromBody] DtoProductBuildInfo productBuildInfo)
    {
        var product = new Product
        {
            Id = id,
            Name = productBuildInfo.Name,
            Logo = productBuildInfo.Logo
        };
        
        var createResult = await sender.Send(new CreateProduct.Request(product));
        if (createResult.TryGetError(out var createdProduct, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return DtoProductMapper.MapFrom(createdProduct);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoProduct>> PutProduct(string id, [FromBody] DtoProductBuildInfo productBuildInfo)
    {
        var product = new Product
        {
            Id = id,
            Name = productBuildInfo.Name,
            Logo = productBuildInfo.Logo
        };
        
        var updateResult = await sender.Send(new UpdateProduct.Request(product));
        if (updateResult.TryGetError(out var updatedProduct, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return DtoProductMapper.MapFrom(updatedProduct);
    }
}