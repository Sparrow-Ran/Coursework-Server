using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("Attachments")]
    public class Attachment
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("issue_id")]
        public int IssueId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("uploaded_by")]
        public int? UploadedBy { get; set; }

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; }

        // Навигационные свойства
        public Issue Issue { get; set; }
        public User Uploader { get; set; }
    }
}
