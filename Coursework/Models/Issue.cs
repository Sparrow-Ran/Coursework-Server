using Coursework.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Issue
{
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("project_id")]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(150)]
    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string? Description { get; set; } // Сделано nullable

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("priority_id")]
    public int? PriorityId { get; set; }

    [Column("assigned_to")]
    public int? AssignedTo { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // Навигационные свойства
    public Project Project { get; set; }
    public IssueStatus? Status { get; set; } // Сделано nullable
    public IssuePriority? Priority { get; set; } // Сделано nullable
    public User? Assignee { get; set; } // Сделано nullable
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}
