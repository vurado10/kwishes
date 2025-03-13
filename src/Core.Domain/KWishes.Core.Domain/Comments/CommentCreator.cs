using KWishes.Core.Domain.Users;

namespace KWishes.Core.Domain.Comments;

public sealed record CommentCreator(
    UserId Id,
    Role Role
);