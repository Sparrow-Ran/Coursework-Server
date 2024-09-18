using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("issue_id")]
        public int IssueId { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства
        public Issue Issue { get; set; }
        public User Author { get; set; }
    }
}
