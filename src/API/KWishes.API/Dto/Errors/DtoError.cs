using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Errors;

public record DtoError(
    [Required] string Code,
    [Required] string Message,
    IReadOnlyDictionary<string, object>? Details = null
);