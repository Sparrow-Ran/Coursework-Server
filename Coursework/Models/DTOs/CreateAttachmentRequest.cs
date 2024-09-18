using System.ComponentModel.DataAnnotations;

namespace Coursework.Models.DTOs
{
    public class CreateAttachmentRequest
    {
        [Required]
        public int IssueId { get; set; }

        [Required]
        public string FilePath { get; set; }
    }
}
