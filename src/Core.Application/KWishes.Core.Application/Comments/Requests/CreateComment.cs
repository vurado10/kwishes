using System.Data;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Caches;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Application.Users.Requests;
using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Comments.Requests;

public static class CreateComment
{
    public record Request(
        UserId UserId,
        WishId WishId,
        string Text,
        IReadOnlyList<Uri> FileUrls
    ) : IResultRequest<CommentId>;

    public class Handler : RepositoryRequestHandler<Request, CommentId>
    {
        private readonly IReadOnlyCache<UserId, User> userById;

        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory, IReadOnlyCache<UserId, User> userById) 
            : base(dbContextFactory)
        {
            this.userById = userById;
        }

        protected override async ValueTask<Result<CommentId>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await userById.Get(request.UserId, cancellationToken);
            if (user is null)
                return Result.Error<CommentId>(UserErrorRegistry.NoWithId(request.UserId));
            var wish = await context
                .Wishes
                .FirstOrDefaultAsync(c => c.Id == request.WishId, cancellationToken);
            
            if (wish is null)
            {
                return Result.Error<CommentId>($"Wish not exists. wish: {request.WishId}");
            }

            var createTime = DateTime.UtcNow;
            var comment = new Comment(
                default,
                request.WishId,
                new CommentCreator(user.Id, user.Role),
                createTime,
                createTime,
                0,
                request.Text,
                request.FileUrls,
                false);

            await using var transaction = await context
                .Database.
                BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            var entry = await context.Comments.AddAsync(comment, cancellationToken);
            wish.CommentCount++;
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result.Ok(entry.Entity.Id);
        }
    }
}