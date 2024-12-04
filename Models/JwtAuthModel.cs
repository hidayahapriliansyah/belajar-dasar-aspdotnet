namespace jwtAuth.Models;

public record UserAccessTokenData(
    int Id,
    string Username,
    string Name,
    string Email,
    string Password,
    string[] Roles
);
