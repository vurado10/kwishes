using System.Security.Claims;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Users.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace KWishes.API.Authentication;

public class AddKWishesUserClaimsTransformation : IClaimsTransformation
{
    private readonly ISender sender;

    public AddKWishesUserClaimsTransformation(ISender sender)
    {
        this.sender = sender;
    }
    
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = await sender.Send(new GetUserByGoogleNameId.Request(principal.GetGoogleNameId()));
        if (result.TryGetError(out var user, out _))
            return principal;

        principal.AddIdentity(user.GetClaimsIdentity());

        return principal;
    }
}