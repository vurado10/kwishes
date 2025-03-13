using System.Security.Claims;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Application.Users.Repository;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Users.Requests;

public static class CreateUserByGoogleAccount
{
    public record Request(ClaimsPrincipal Principal) : IResultRequest<UserId>;
    
    public class Handler 
        : RepositoryRequestHandler<Request, UserId>
    {
        public Handler(IDbContextFactory<RepositoryContext> dbContextFactory) 
            : base(dbContextFactory)
        {
        }
    
        protected override async ValueTask<Result<UserId>> Handle(
            RepositoryContext context, 
            Request request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.GetUserByGoogleNameId(
                request.Principal.GetGoogleNameId(), 
                cancellationToken);
            
            if (user is not null)
                return Result.Error<UserId>($"User already exists. GoogleNameId: {user.GoogleNameId}");
        
            var entry = await context.Users.AddAsync(MapUser(request.Principal), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        
            return Result.Ok(entry.Entity.Id);
        }

        private static User MapUser(ClaimsPrincipal claimsPrincipal)
        { 
            return new User(
                default,
                claimsPrincipal.GetGoogleName(),
                claimsPrincipal.GetGoogleGivenName(),
                claimsPrincipal.GetGooleSurnameName(),
                new EmailAddress(claimsPrincipal.GetGoogleEmailAddressName()),
                Role.User, 
                null,
                claimsPrincipal.GetGoogleNameId());
        }
    }
}
