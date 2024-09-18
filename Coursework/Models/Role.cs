using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("Roles")]
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        // Навигационные свойства
        public ICollection<User> Users { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
