using KWishes.API.Dto.Users;
using KWishes.Core.Domain.Wishes;

namespace KWishes.API.Dto.Wishes;

public static class DtoWishMapper
{
    public static DtoWish MapFrom(Wish wish, bool isLiked)
    {
        return new DtoWish(
            wish.Id,
            wish.ProductId,
            new DtoShortUser(wish.Creator.Id, wish.Creator.FirstName, wish.Creator.SecondName, wish.Creator.Role),
            wish.CreatedAt,
            wish.Status,
            wish.CommentCount,
            wish.VoteCount,
            wish.LastUpdateAt,
            wish.Text,
            isLiked);
    }
}