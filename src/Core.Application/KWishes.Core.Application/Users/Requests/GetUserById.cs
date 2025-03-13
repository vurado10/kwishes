using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Requests;

public static class GetUserById
{
    public record Request(UserId Id) : IResultRequest<User>;

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
            var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);
        
            return user is null 
                ? Result.Error<User>($"No user with id: {request.Id.Value}") 
                : Result.Ok(user);
        }
    }
}
