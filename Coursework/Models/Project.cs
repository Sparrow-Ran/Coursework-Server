using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("Projects")]
    public class Project
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; } // Сделано nullable

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("manager_id")]
        public int? ManagerId { get; set; }

        // Навигационные свойства
        public User? Manager { get; set; } // Сделано nullable
        public ICollection<Issue> Issues { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
