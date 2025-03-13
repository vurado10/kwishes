namespace KWishes.Core.Domain.Comments;

public record struct CommentId(Guid Value)
{
    public static implicit operator Guid(CommentId userId) => userId.Value;
}
