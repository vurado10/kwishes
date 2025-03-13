namespace KWishes.Core.Domain.Wishes;

public record struct WishId(Guid Value)
{
    public static implicit operator Guid(WishId userId) => userId.Value;
}
