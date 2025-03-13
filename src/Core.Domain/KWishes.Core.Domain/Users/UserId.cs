namespace KWishes.Core.Domain.Users;

public record struct UserId(Guid Value)
{
    public static implicit operator Guid(UserId userId) => userId.Value;
}
