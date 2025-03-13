using KWishes.Core.Domain.Products;
using KWishes.Core.Domain.Users;

namespace KWishes.Core.Domain.Wishes;

public sealed record Wish
{

    public WishId Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public WishStatus Status { get; set; }
    public int CommentCount { get; set; }
    public int VoteCount { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsVisibleForUsers { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public string Text { get; set; }
    
    public string ProductId { get; init; }
    public Product Product { get; set; }
    
    public UserId CreatorId { get; init; }
    public User Creator { get; init; }
}