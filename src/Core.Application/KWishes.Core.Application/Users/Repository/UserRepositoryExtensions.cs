using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KWishes.Core.Application.Users.Repository;

public static class UserRepositoryExtensions
{
    public static Task<User?> GetUserByGoogleNameId(
        this IQueryable<User> users,
        string id,
        CancellationToken cancellationToken)
    {
        return users.FirstOrDefaultAsync(
            user => user.GoogleNameId == id, 
            cancellationToken);
    }
    
    public static EntityEntry<Comment> Create(
        this DbSet<Comment> comments,
        WishId wishId,
        User user,
        string text)
    {
        var createdAt = DateTime.UtcNow;
        
        return comments.Add(new Comment(
            default, 
            wishId, 
            new CommentCreator(user.Id, user.Role),
            createdAt, 
            createdAt,
            0,
            text,
            Array.Empty<Uri>(),
            false));
    }
}