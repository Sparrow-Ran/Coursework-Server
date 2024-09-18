namespace Coursework.Models.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO Author { get; set; }
    }
}
