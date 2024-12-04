using System.Linq.Expressions;
using jwtAuth.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace jwtAuth.Database;

public class ThreadDbContext(DbContextOptions<ThreadDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<ThreadPost> ThreadPost { get; set; } = null!;
    public DbSet<Comment> Comment { get; set; } = null!;
    public DbSet<Reply> Reply { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        GenerateUuid<User>(modelBuilder, "Id");
        SoftDelete<User>(modelBuilder);
        DefaultCreatedAtGetDate<User>(modelBuilder);

        GenerateUuid<ThreadPost>(modelBuilder, "Id");
        SoftDelete<ThreadPost>(modelBuilder);
        DefaultCreatedAtGetDate<ThreadPost>(modelBuilder);

        GenerateUuid<Comment>(modelBuilder, "Id");
        SoftDelete<Comment>(modelBuilder);
        DefaultCreatedAtGetDate<Comment>(modelBuilder);

        GenerateUuid<Reply>(modelBuilder, "Id");
        SoftDelete<Reply>(modelBuilder);
        DefaultCreatedAtGetDate<Reply>(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(c => c.Username).IsUnique();

        modelBuilder
            .Entity<ThreadPost>()
            .HasOne(t => t.User)
            .WithMany(u => u.ThreadPosts)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Comment>()
            .HasOne(c => c.ThreadPost)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.ThreadPostId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Reply>()
            .HasOne(r => r.Comment)
            .WithMany(c => c.Replies)
            .HasForeignKey(r => r.CommentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Reply>()
            .HasOne(r => r.User)
            .WithMany(u => u.Replies)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        Seeder(modelBuilder);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is Base
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
            {
                ((Base)entityEntry.Entity).UpdatedAt = DateTime.Now;
            }

            if (entityEntry.State == EntityState.Added)
            {
                ((Base)entityEntry.Entity).CreatedAt = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    private static void GenerateUuid<T>(ModelBuilder modelBuilder, string column)
        where T : Base
    {
        modelBuilder.Entity<T>().HasIndex(CreateExpression<T>(column));

        modelBuilder.Entity<T>().Property(column).HasDefaultValueSql("NEWID()");
    }

    private static void DefaultCreatedAtGetDate<T>(ModelBuilder modelBuilder)
        where T : Base
    {
        modelBuilder.Entity<T>().Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
    }

    private static Expression<Func<T, object?>> CreateExpression<T>(string uuid)
        where T : Base
    {
        var type = typeof(T);
        var property = type.GetProperty(uuid);
        var parameter = Expression.Parameter(type);
        Expression access;
        if (property != null)
        {
            access = Expression.Property(parameter, property);
        }
        else
        {
            throw new ArgumentException($"Property '{uuid}' not found on type '{type}'.");
        }
        var convert = Expression.Convert(access, typeof(object));
        var function = Expression.Lambda<Func<T, object?>>(convert, parameter);

        return function;
    }

    private static void SoftDelete<T>(ModelBuilder modelBuilder)
        where T : Base
    {
        modelBuilder
            .Entity<T>()
            .HasQueryFilter(e => EF.Property<DateTime?>(e, "DeletedAt") == null);
    }

    private static void Seeder(ModelBuilder modelBuilder)
    {
        Guid IdUser1 = Guid.NewGuid();
        Guid IdUser2 = Guid.NewGuid();

        Guid IdThreadPost1 = Guid.NewGuid();
        Guid IdThreadPost2 = Guid.NewGuid();

        Guid IdComment1 = Guid.NewGuid();
        Guid IdComment2 = Guid.NewGuid();

        Guid IdReply1 = Guid.NewGuid();
        Guid IdReply2 = Guid.NewGuid();

        modelBuilder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = IdUser1,
                    FullName = "John Doe",
                    Username = "john.doe",
                },
                new User
                {
                    Id = IdUser2,
                    FullName = "Jane Smith",
                    Username = "jane.smith@example.com",
                }
            );

        modelBuilder
            .Entity<ThreadPost>()
            .HasData(
                new ThreadPost
                {
                    Id = IdThreadPost1,
                    Body = "Introduction to Entity Framework",
                    UserId = IdUser1, // Gantilah dengan ID User yang valid
                },
                new ThreadPost
                {
                    Id = IdThreadPost2,
                    Body = "Getting Started with C#",
                    UserId = IdUser2, // Gantilah dengan ID User yang valid
                }
            );

        modelBuilder
            .Entity<Comment>()
            .HasData(
                new Comment
                {
                    Id = IdComment1,
                    Content = "This is a great thread, very helpful!",
                    ThreadPostId = IdThreadPost1,
                    UserId = IdUser2,
                },
                new Comment
                {
                    Id = IdComment2,
                    Content = "I have some questions about EF, can you explain more?",
                    ThreadPostId = IdThreadPost1,
                    UserId = IdUser1,
                }
            );

        modelBuilder
            .Entity<Reply>()
            .HasData(
                new Reply
                {
                    Id = IdReply1,
                    Content = "Sure, I'd be happy to explain more about EF.",
                    CommentId = IdComment1,
                    UserId = IdUser1,
                },
                new Reply
                {
                    Id = IdReply2,
                    Content = "Can you provide an example of how to set up EF?",
                    CommentId = IdComment1,
                    UserId = IdUser2,
                }
            );
    }
}
