using Coursework.Models.DTOs;
using Coursework.Models;

namespace Coursework.Models.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDTO? Manager { get; set; } // Сделано nullable
    }
}
