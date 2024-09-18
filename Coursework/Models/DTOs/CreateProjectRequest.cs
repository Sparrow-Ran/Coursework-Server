using System.ComponentModel.DataAnnotations;

namespace Coursework.Models.DTOs
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
