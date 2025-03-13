using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Wishes;

public record DtoWishUpdateInfo(
    [Required] string Text
);