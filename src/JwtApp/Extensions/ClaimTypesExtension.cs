using System.Security.Claims;

namespace JwtApp.Extensions;

public static class ClaimTypesExtension
{
    public static int Id(this ClaimsPrincipal claims)
        => int.Parse(claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

    public static string Name(this ClaimsPrincipal claims)
        => claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;

    public static string Email(this ClaimsPrincipal claims)
        => claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;

    public static string Image(this ClaimsPrincipal claims)
        => claims.Claims.FirstOrDefault(x => x.Type == "image")?.Value ?? string.Empty;
}
