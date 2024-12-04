using jwtAuth.Exceptions;
using jwtAuth.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace jwtAuth.Filters;

public class RoleAuthorizeAttribute(params string[] roles) : ActionFilterAttribute
{
    private readonly string[] _allowedRoles = roles;

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("Invoked role auth ...");

        context.HttpContext.Items.TryGetValue("User", out var user);
        var userRoles = (user as UserAccessTokenData)?.Roles ?? [""];
        if (!(userRoles.Any(role => _allowedRoles.Contains(role))))
        {
            Console.WriteLine("Role match found.");
            throw new ForbiddenException();
        }
        base.OnResultExecuting(context);
    }
}
