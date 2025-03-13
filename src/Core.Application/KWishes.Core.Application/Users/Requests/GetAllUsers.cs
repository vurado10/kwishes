using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Requests;

public static class GetAllUsers
{
    public record Request : IResultRequest<IReadOnlyList<User>>;

    public class Handler : RepositoryRequestHandler<Request, IReadOnlyList<User>>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<IReadOnlyList<User>>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.ToArrayAsync(cancellationToken);
            return Result.Ok<IReadOnlyList<User>>(user);
        }
    }
}
