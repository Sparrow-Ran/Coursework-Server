namespace Coursework.Models.DTOs
{
    public class IssueDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } // Сделано nullable
        public IssueStatusDTO? Status { get; set; } // Сделано nullable
        public IssuePriorityDTO? Priority { get; set; } // Сделано nullable
        public UserDTO? Assignee { get; set; } // Сделано nullable
    }
}
