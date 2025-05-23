using System.Security.Claims;

namespace FileFlow.Api;

public static class ClaimsPrincipalException
{
    public static string GetUserid(this ClaimsPrincipal source)
    {
        var userIdClaim = source.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
        {
            throw new InvalidOperationException($"{ClaimTypes.GivenName} claim not found");
        }
        
        return userIdClaim.Value;
    }
}