using System.Data;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Votes;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Votes.Requests;

public class CreateCommentVote
{
    public record Request(Vote Vote) : IResultRequest<Vote>;

    public class Handler : RepositoryRequestHandler<Request, Vote>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Vote>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var vote =  await context.Votes.FirstOrDefaultAsync(
                v =>
                    v.CreatorId == request.Vote.CreatorId
                    && v.EntityId == request.Vote.EntityId
                    && v.Type == request.Vote.Type,
                cancellationToken);
            if (vote is not null)
            {
                return Result.Error<Vote>($"Vote  already exists. CreatorId: {request.Vote.CreatorId}, EntityId: {request.Vote.EntityId}");
            }

            var comment = await context
                .Comments
                .FirstOrDefaultAsync(c => c.Id == request.Vote.EntityId, cancellationToken);
            
            if (comment is null)
            {
                return Result.Error<Vote>($"Comment not exists. Comment: {request.Vote.EntityId}");
            }

            await using var transaction = await context
                .Database.
                BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            var newVote = new Vote
            {
                EntityId = request.Vote.EntityId,
                Type = request.Vote.Type,
                CreatorId = request.Vote.CreatorId
            };
            var entry = await context.Votes.AddAsync(newVote, cancellationToken);
            comment.VoteCount++;
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            
            return Result.Ok(entry.Entity);
        }
    }
}