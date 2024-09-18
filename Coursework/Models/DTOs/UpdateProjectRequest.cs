using System.ComponentModel.DataAnnotations;
public class UpdateProjectRequest
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}
