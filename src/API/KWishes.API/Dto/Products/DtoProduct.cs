using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Products;

public record DtoProduct(
    [Required] string Id,
    [Required] string Name,
    [Required] Uri Logo
);
