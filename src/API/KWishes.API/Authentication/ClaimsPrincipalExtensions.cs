using System.Security.Claims;
using KWishes.Core.Domain.Users;

namespace KWishes.API.Authentication;

public static class KWishesUserClaimInfo
{
    public const string ClaimsIdentityName = "KWishesUser";
    
    public static class ClaimTypes
    {
        public const string IdentityName = "KWishesUserIdentity";
        public const string UserId = "UserId";
        public const string Role = "KWishesUserId";
    }
}

public static class ClaimsPrincipalExtensions
{
    public static ClaimsIdentity GetClaimsIdentity(this User user)
    {
        var identity = new ClaimsIdentity(null, KWishesUserClaimInfo.ClaimTypes.IdentityName, null);
        
        identity.AddClaim(
            new Claim(KWishesUserClaimInfo.ClaimTypes.IdentityName, KWishesUserClaimInfo.ClaimsIdentityName));
        
        identity.AddClaim(new Claim(KWishesUserClaimInfo.ClaimTypes.UserId, user.Id.Value.ToString("N")));
        identity.AddClaim(new Claim(KWishesUserClaimInfo.ClaimTypes.Role, user.Role.ToString("G")));

        return identity;
    }
    
    public static UserId GetKWishesUserId(this ClaimsPrincipal principal)
    {
        var userIdStr = principal.Identities
            .FirstOrDefault(identity => identity.Name == KWishesUserClaimInfo.ClaimsIdentityName)
            ?.FindFirst(claim => claim.Type == KWishesUserClaimInfo.ClaimTypes.UserId)
            ?.Value;

        if (!Guid.TryParse(userIdStr, out var userIdGuid))
        {
            throw new InvalidOperationException(
                $"No claim with name '{KWishesUserClaimInfo.ClaimsIdentityName}' " +
                $"and type '{KWishesUserClaimInfo.ClaimTypes.UserId}'");
        }
        
        return new UserId(userIdGuid);
    }
    
    public static Role GetKWishesUserRole(this ClaimsPrincipal principal)
    {
        var roleStr = principal.Identities
            .FirstOrDefault(identity => identity.Name == KWishesUserClaimInfo.ClaimsIdentityName)
            ?.FindFirst(claim => claim.Type == KWishesUserClaimInfo.ClaimTypes.Role)
            ?.Value;

        if (!Enum.TryParse<Role>(roleStr, out var role))
        {
            throw new InvalidOperationException(
                $"No claim with name '{KWishesUserClaimInfo.ClaimsIdentityName}' " +
                $"and type '{KWishesUserClaimInfo.ClaimTypes.Role}'");
        }
        
        return role;
    }
}