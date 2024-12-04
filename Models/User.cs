using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwtAuth.Models;

[Table(name: "users")]
public class User : Base
{
    [Key]
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "fullname")]
    public string FullName { get; set; } = null!;

    [Index(IsUnique = true)]
    [Column(name: "username")]
    public string Username { get; set; } = null!;

    public ICollection<ThreadPost> ThreadPosts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Reply> Replies { get; set; } = [];
}
