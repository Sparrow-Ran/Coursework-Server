using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("ProjectMembers")]
    public class ProjectMember
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("project_id")]
        public int ProjectId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        // Навигационные свойства
        public Project Project { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
