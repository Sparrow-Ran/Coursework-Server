using System.ComponentModel.DataAnnotations;

public class CreateIssueRequest
{
    [Required]
    public int ProjectId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
    public int? StatusId { get; set; }
    public int? PriorityId { get; set; }
    public int? AssignedTo { get; set; }
}