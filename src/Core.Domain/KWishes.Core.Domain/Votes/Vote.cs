using KWishes.Core.Domain.Users;

namespace KWishes.Core.Domain.Votes;

public record Vote
{
    public Guid EntityId {get; init;}
    public VoteType Type { get; init; }
    public UserId CreatorId { get; init; }
    public User Creator { get; init; }
}