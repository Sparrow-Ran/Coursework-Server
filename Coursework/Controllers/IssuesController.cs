using Coursework.Models.DTOs;
using Coursework.Models;
using Coursework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class IssuesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IssuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Создает новую задачу.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<IssueDTO>> Create([FromBody] CreateIssueRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Проверка, что проект существует
        var project = await _context.Projects.FindAsync(model.ProjectId);
        if (project == null)
        {
            return BadRequest("Проект не найден.");
        }

        var issue = new Issue
        {
            ProjectId = model.ProjectId,
            Title = model.Title,
            Description = model.Description,
            StatusId = model.StatusId,
            PriorityId = model.PriorityId,
            AssignedTo = model.AssignedTo,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        var issueDTO = await GetIssueDTO(issue.Id);

        return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issueDTO);
    }

    /// <summary>
    /// Получает задачу по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<IssueDTO>> GetById(int id)
    {
        var issueDTO = await GetIssueDTO(id);

        if (issueDTO == null)
        {
            return NotFound();
        }

        return Ok(issueDTO);
    }

    /// <summary>
    /// Обновляет задачу.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateIssueRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
        {
            return NotFound();
        }

        // Проверка прав доступа (например, только менеджер проекта или назначенный пользователь может обновлять задачу)
        var userId = int.Parse(User.FindFirst("id").Value);
        var project = await _context.Projects.FindAsync(issue.ProjectId);

        if (project.ManagerId != userId && issue.AssignedTo != userId)
        {
            return Forbid();
        }

        issue.Title = model.Title;
        issue.Description = model.Description;
        issue.StatusId = model.StatusId;
        issue.PriorityId = model.PriorityId;
        issue.AssignedTo = model.AssignedTo;
        issue.UpdatedAt = DateTime.UtcNow;

        _context.Entry(issue).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Issues.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Удаляет задачу.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
        {
            return NotFound();
        }

        // Проверка прав доступа
        var userId = int.Parse(User.FindFirst("id").Value);
        var project = await _context.Projects.FindAsync(issue.ProjectId);

        if (project.ManagerId != userId)
        {
            return Forbid();
        }

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Получает задачи по проекту.
    /// </summary>
    [HttpGet("Project/{projectId}")]
    public async Task<ActionResult<IEnumerable<IssueDTO>>> GetIssuesByProject(int projectId)
    {
        var issues = await _context.Issues
            .Where(i => i.ProjectId == projectId)
            .Include(i => i.Status)
            .Include(i => i.Priority)
            .Include(i => i.Assignee)
            .ToListAsync();

        var issueDTOs = issues.Select(issue => new IssueDTO
        {
            Id = issue.Id,
            ProjectId = issue.ProjectId,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status != null ? new IssueStatusDTO { Id = issue.Status.Id, Name = issue.Status.Name } : null,
            Priority = issue.Priority != null ? new IssuePriorityDTO { Id = issue.Priority.Id, Name = issue.Priority.Name } : null,
            Assignee = issue.Assignee != null ? new UserDTO { Id = issue.Assignee.Id, Username = issue.Assignee.Username, Email = issue.Assignee.Email } : null
        });

        return Ok(issueDTOs);
    }

    private async Task<IssueDTO> GetIssueDTO(int id)
    {
        var issue = await _context.Issues
            .Include(i => i.Status)
            .Include(i => i.Priority)
            .Include(i => i.Assignee)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
        {
            return null;
        }

        var issueDTO = new IssueDTO
        {
            Id = issue.Id,
            ProjectId = issue.ProjectId,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status != null ? new IssueStatusDTO { Id = issue.Status.Id, Name = issue.Status.Name } : null,
            Priority = issue.Priority != null ? new IssuePriorityDTO { Id = issue.Priority.Id, Name = issue.Priority.Name } : null,
            Assignee = issue.Assignee != null ? new UserDTO { Id = issue.Assignee.Id, Username = issue.Assignee.Username, Email = issue.Assignee.Email } : null
        };

        return issueDTO;
    }
}
