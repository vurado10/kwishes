using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Requests;

public static class GetUserByGoogleNameId
{
    public record Request(string GoogleNameId) : IResultRequest<User>;

    public class Handler : RepositoryRequestHandler<Request, User>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<User>> Handle(
            RepositoryContext context,
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(
                user => user.GoogleNameId == request.GoogleNameId,
                cancellationToken);

            return user is null 
                ? Result.Error<User>($"No user with GoogleNameId: {request.GoogleNameId}") 
                : Result.Ok(user);
        }
    }
}
