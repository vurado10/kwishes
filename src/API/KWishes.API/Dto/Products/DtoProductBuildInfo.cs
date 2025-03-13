using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Products;

public record DtoProductBuildInfo(
    [Required] string Name,
    [Required] Uri Logo
);