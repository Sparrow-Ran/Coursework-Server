using System.ComponentModel.DataAnnotations;

namespace Coursework.Models.DTOs
{
    public class CreateCommentRequest
    {
        [Required]
        public int IssueId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
