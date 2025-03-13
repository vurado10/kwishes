using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Requests;

public static class UpdateUser
{
    public record Request(Guid Id, Role? Role) : IResultRequest<Nothing>;

    public class Handler : RepositoryRequestHandler<Request, Nothing>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }

        protected override async ValueTask<Result<Nothing>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user is null)
                return Result.Error<Nothing>($"No user with id: {request.Id}");

            var count = await context.Users
                .Where(u => u.Id == request.Id)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(u => u.Role, request.Role),
                    cancellationToken);

            return count <= 0 ? Result.Error<Nothing>("No users") : Result.Ok();
        }
    }
}