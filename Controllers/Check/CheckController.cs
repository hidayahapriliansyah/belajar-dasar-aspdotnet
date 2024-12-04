using jwtAuth.Filters;
using Microsoft.AspNetCore.Mvc;

namespace jwtAuth.Controllers.Check;

[Route("/api/check")]
[ApiController]
public class CheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("OK!");
    }

    [HttpGet("tech")]
    [RoleAuthorize(["manager"])]
    public IActionResult GetTech()
    {
        var user = HttpContext.Items["User"];

        Console.WriteLine("User on controller => " + user);
        Console.WriteLine("User.GetType on controller => " + user!.GetType());

        return Ok("OK Tech!");
    }
}
