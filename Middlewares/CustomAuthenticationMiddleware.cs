using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jwtAuth.Exceptions;
using jwtAuth.Helper;
using jwtAuth.Models;
using Microsoft.IdentityModel.Tokens;

namespace jwtAuth.Middlewares;

public class CustomAuthenticationMiddleware : IMiddleware
{
    private readonly string _secretKey = Configuration.PrivateKey;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (
            !context.Request.Headers.TryGetValue("Authorization", out var tokenHeader)
            || !tokenHeader.ToString().StartsWith("Bearer ")
        )
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing or invalid Authorization header");
            return;
        }

        var token = tokenHeader.ToString()["Bearer ".Length..].Trim();

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                },
                out var validatedToken
            );

            var jwtToken = (JwtSecurityToken)validatedToken;

            Console.WriteLine("jwtToken =>" + jwtToken);
            Console.WriteLine("jwtToken Claims =>" + jwtToken.Claims);

            UserAccessTokenData user = jwtToken.ToUserAccessTokenData();
            context.Items["User"] = user;
        }
        catch
        {
            Console.WriteLine("Catch throw error invoked ...");
            throw new UnauthenticatedException();
        }
        await next(context);
    }
}
