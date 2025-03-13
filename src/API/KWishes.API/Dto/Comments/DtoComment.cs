using System.ComponentModel.DataAnnotations;
using KWishes.API.Dto.Users;

namespace KWishes.API.Dto.Comments;

public record DtoComment(
    [Required] Guid Id,
    [Required] Guid WishId,
    [Required] DtoShortUser Creator,
    [Required] DateTime LastUpdateAt,
    [Required] DateTime CreatedAt,
    [Required] long VoteCount,
    [Required] string Text,
    [Required] IReadOnlyList<Uri> FileUrls
);