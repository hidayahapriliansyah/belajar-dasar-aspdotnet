using System.IdentityModel.Tokens.Jwt;
using jwtAuth.Models;

namespace jwtAuth.Helper;

public static class JwtExtensions
{
    public static UserAccessTokenData ToUserAccessTokenData(this JwtSecurityToken jwtToken)
    {
        var id = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;
        var name = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? string.Empty;
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
        var roles = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToArray();

        var user = new UserAccessTokenData(
            Id: id,
            Username: username,
            Name: name,
            Email: email,
            Password: string.Empty,
            Roles: roles
        );

        return user;
    }
}
