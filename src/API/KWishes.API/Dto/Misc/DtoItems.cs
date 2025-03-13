using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Misc;

public record DtoItems<TItem>([Required] IReadOnlyList<TItem> Items);