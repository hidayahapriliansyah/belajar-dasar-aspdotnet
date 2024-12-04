using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwtAuth.Models;

[Table("replies")]
public class Reply : Base
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

    [ForeignKey("Comment")]
    [Column(name: "comment_id")]
    public Guid CommentId { get; set; }

    public User User { get; set; } = null!;
    public Comment Comment { get; set; } = null!;
}
