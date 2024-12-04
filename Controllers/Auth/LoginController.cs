using System.Net;
using jwtAuth.Helper;
using jwtAuth.Models;
using jwtAuth.Response.Auth;
using jwtAuth.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace jwtAuth.Controllers;

[Route("/api/auth/login")]
[ApiController]
public class LoginController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpGet]
    public ApiResponse<LoginResponse> Login()
    {
        var user = new UserAccessTokenData(
            1,
            "bruno.bernardes",
            "Bruno Bernardes",
            "bruno@gmail.com",
            "q1w2e3r4t5",
            ["engineer", "developer"]
        );

        var accessToken = _authService.Create(user);

        var dto = new LoginResponse() { AccessToken = accessToken };

        var response = new ApiResponse<LoginResponse>(
            HttpStatusCode.OK,
            dto,
            "Berhasil mendapatkan access token."
        );

        return response;
    }
}
