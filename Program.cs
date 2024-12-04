using jwtAuth.Database;
using jwtAuth.Middlewares;
using jwtAuth.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddTransient<CustomAuthenticationMiddleware>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<ThreadDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseExceptionHandler();

app.UseWhen(
    context =>
    {
        if (
            context.Request.Path.StartsWithSegments("/api/auth/login")
            || context.Request.Path.StartsWithSegments("/api/thread-post")
            || context.Request.Path.StartsWithSegments("/swagger")
        )
        {
            return false;
        }
        else
        {
            return true;
        }
    },
    appBuilder =>
    {
        appBuilder.UseMiddleware<CustomAuthenticationMiddleware>();
    }
);

app.MapControllers();

app.Run();
