using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("IssuePriorities")]
    public class IssuePriority
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        // Навигационные свойства
        public ICollection<Issue> Issues { get; set; }
    }
}
