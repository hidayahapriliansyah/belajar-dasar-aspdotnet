using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwtAuth.Models;

[Table(name: "thread_posts")]
public class ThreadPost : Base
{
    [Key]
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "body")]
    public string Body { get; set; } = null!;

    [Column(name: "is_deleted")]
    public bool IsDeleded { get; set; } = false;

    [ForeignKey("User")]
    [Column(name: "user_id")]
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
}
