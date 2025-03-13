using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Votes.Requests;

public class IsVoteExist
{
    public record Request(Guid VoteId, UserId UserId, VoteType Type) : IResultRequest<bool>;

    public class Handler : RepositoryRequestHandler<Request, bool>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<bool>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var vote = await context.Votes
                .FirstOrDefaultAsync(vote =>
                    vote.EntityId == request.VoteId
                    && vote.CreatorId == request.UserId
                    && vote.Type == request.Type, cancellationToken);
            
            return Result.Ok(vote is not null);
        }
    }
}