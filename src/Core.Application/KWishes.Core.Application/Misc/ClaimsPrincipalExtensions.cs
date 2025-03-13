using System.Security.Claims;

namespace KWishes.Core.Application.Misc;

public static class ClaimsPrincipalExtensions
{
    private const string GoogleNameIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    private const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    private const string GivenNameIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
    private const string SurnameIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
    private const string EmailAddressIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    
    public static string GetGoogleNameId(this ClaimsPrincipal principal) => 
        principal.Claims.First(claim => claim.Type == GoogleNameIdClaimType).Value;
    
    public static string GetGoogleName(this ClaimsPrincipal principal) => 
        principal.Claims.First(claim => claim.Type == NameClaimType).Value;
    
    public static string GetGoogleGivenName(this ClaimsPrincipal principal) => 
        principal.Claims.First(claim => claim.Type == GivenNameIdClaimType).Value;
    
    public static string GetGooleSurnameName(this ClaimsPrincipal principal) => 
        principal.Claims.First(claim => claim.Type == SurnameIdClaimType).Value;
    
    public static string GetGoogleEmailAddressName(this ClaimsPrincipal principal) => 
        principal.Claims.First(claim => claim.Type == EmailAddressIdClaimType).Value;
}