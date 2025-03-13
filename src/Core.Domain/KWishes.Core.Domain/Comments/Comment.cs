using KWishes.Core.Domain.Wishes;

namespace KWishes.Core.Domain.Comments;

public sealed record Comment
{
    public Comment(
        CommentId id,
        WishId wishId,
        CommentCreator creator,
        DateTime createdAt,
        DateTime lastUpdateAt,
        long voteCount,
        string text,
        IReadOnlyList<Uri> files,
        bool isDeleted)
    {
        Id = id;
        WishId = wishId;
        Creator = creator;
        CreatedAt = createdAt;
        LastUpdateAt = lastUpdateAt;
        VoteCount = voteCount;
        Text = text;
        Files = files;
        IsDeleted = isDeleted;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private Comment()
#pragma warning restore CS8618
    {
    }

    public CommentId Id { get; init; }
    public WishId WishId { get; init; }
    public CommentCreator Creator { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdateAt { get; init; }
    public long VoteCount { get; set; }
    public string Text { get; init; }
    public IReadOnlyList<Uri> Files { get; init; }
    public bool IsDeleted { get; init; }
}