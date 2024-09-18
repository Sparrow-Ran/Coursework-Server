namespace Coursework.Models.DTOs
{
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public UserDTO Uploader { get; set; }
    }
}
