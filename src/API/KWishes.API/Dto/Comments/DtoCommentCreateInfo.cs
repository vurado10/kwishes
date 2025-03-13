using System.ComponentModel.DataAnnotations;

namespace KWishes.API.Dto.Comments;

public record DtoCommentCreateInfo(
    [Required] Guid WishId,
    [Required] string Text,
    [Required] IReadOnlyList<Uri> FileUrls
);