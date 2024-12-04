using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwtAuth.Models;

[Table(name: "comments")]
public class Comment : Base
{
    [Key]
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "content")]
    public string Content { get; set; } = null!;

    [Column(name: "is_deleted")]
    public bool IsDeleded { get; set; } = false;

    [ForeignKey("User")]
    [Column(name: "user_id")]
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    [ForeignKey("ThreadPost")]
    [Column("thread_post_id")]
    public Guid ThreadPostId { get; set; }

    public ThreadPost ThreadPost { get; set; } = null!;
    public ICollection<Reply> Replies { get; set; } = [];
}
