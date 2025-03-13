using KWishes.API.Dto.Misc;
using KWishes.API.Dto.Users;
using KWishes.Core.Application.Misc.Caches;
using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Users;

namespace KWishes.API.Dto.Comments;

public class DtoCommentMapper
{
    private readonly IReadOnlyCache<UserId, User> userById;

    public DtoCommentMapper(IReadOnlyCache<UserId, User> userById)
    {
        this.userById = userById;
    }
    
    public async ValueTask<DtoItems<DtoComment>> MapFrom(IReadOnlyList<Comment> comments)
    {
        var result = new List<DtoComment>(comments.Count);
        
        foreach (var comment in comments)
        {
            var dtoComment = await MapFrom(comment);
            result.Add(dtoComment);
        }
        
        return new DtoItems<DtoComment>(result);
    }
    
    public async ValueTask<DtoComment> MapFrom(Comment comment)
    {
        var user = await userById.Get(comment.Creator.Id);

        return new DtoComment(
            comment.Id.Value,
            comment.WishId.Value,
            new DtoShortUser(
                comment.Creator.Id, 
                user?.FirstName ?? "Deleted",
                user?.SecondName ?? "Deleted",
                comment.Creator.Role), 
            comment.LastUpdateAt, 
            comment.CreatedAt,
            comment.VoteCount,
            comment.Text,
            comment.Files);
    }
}