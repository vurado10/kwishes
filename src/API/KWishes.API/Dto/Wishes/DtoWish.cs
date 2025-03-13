using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using KWishes.API.Dto.Users;
using KWishes.Core.Domain.Wishes;

namespace KWishes.API.Dto.Wishes;

public record DtoWish(
    [Required] Guid Id,
    [Required] string ProductId,
    [Required] DtoShortUser Creator,
    [Required] DateTime CreatedAt,
    [Required] WishStatus Status,
    [Required] int CommentCount,
    [Required] int VoteCount,
    [Required] DateTime LastUpdateAt,
    [Required] string Text,
    [Optional] bool IsLikeByCurrent
);