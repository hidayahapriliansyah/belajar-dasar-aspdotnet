using jwtAuth.Database;
using jwtAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jwtAuth.Controllers.THreadPostController;

[Route("/api/thread-post")]
[ApiController]
public class ThreadPostController(ThreadDbContext dbContext) : ControllerBase
{
    private readonly ThreadDbContext _dbContext = dbContext;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Thread post ok");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ThreadPost>> GetThreadPost(Guid id)
    {
        var threadPost = await _dbContext
            .ThreadPost.Include(tp => tp.User)
            // .ThreadPost.Include(tp => tp.Comments)
            // .ThenInclude(c => c.Replies) // Mengambil Replies terkait dengan Comment
            .Where(tp => tp.Id == id)
            .FirstOrDefaultAsync();

        if (threadPost == null)
        {
            return NotFound();
        }

        return Ok(threadPost);
    }
}
