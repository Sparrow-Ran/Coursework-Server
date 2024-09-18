using Coursework.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public Role? Role { get; set; } // Сделано nullable
    public ICollection<ProjectMember> ProjectMembers { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}
