using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Wishes;

public record DtoRejectCreateInfo(
    [Required] string Text
);