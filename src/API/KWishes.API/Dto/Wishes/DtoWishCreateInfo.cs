using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Wishes;

public record DtoWishCreateInfo(
    [Required] string ProductId,
    [Required] string Text
);