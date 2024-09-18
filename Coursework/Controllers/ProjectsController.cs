using Coursework.Models.DTOs;
using Coursework.Models;
using Coursework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Создает новый проект.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProjectDTO>> Create([FromBody] CreateProjectRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Не удалось определить пользователя.");
        }

        var project = new Project
        {
            Name = model.Name,
            CreatedAt = DateTime.UtcNow,
            ManagerId = userId
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Загрузка информации о менеджере
        var manager = await _context.Users.FindAsync(userId);
        if (manager == null)
        {
            return StatusCode(500, "Не удалось найти менеджера проекта.");
        }

        var projectDTO = new ProjectDTO
        {
            Id = project.Id,
            Name = project.Name,
            Manager = new UserDTO
            {
                Id = manager.Id,
                Username = manager.Username,
                Email = manager.Email
            }
        };

        return CreatedAtAction(nameof(GetById), new { id = project.Id }, projectDTO);
    }


    /// <summary>
    /// Получает проект по его идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> GetById(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Manager)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        var projectDTO = new ProjectDTO
        {
            Id = project.Id,
            Name = project.Name,
            Manager = new UserDTO
            {
                Id = project.Manager.Id,
                Username = project.Manager.Username,
                Email = project.Manager.Email
            }
        };

        return Ok(projectDTO);
    }

    /// <summary>
    /// Обновляет существующий проект.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        // Проверка, является ли текущий пользователь менеджером проекта
        var userId = int.Parse(User.FindFirst("id").Value);
        if (project.ManagerId != userId)
        {
            return Forbid();
        }

        project.Name = model.Name;
        project.Description = model.Description;

        _context.Entry(project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Projects.Any(e => e.Id == id))
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
    /// Удаляет проект.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        // Проверка, является ли текущий пользователь менеджером проекта
        var userId = int.Parse(User.FindFirst("id").Value);
        if (project.ManagerId != userId)
        {
            return Forbid();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Получает список проектов текущего пользователя.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetUserProjects()
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Не удалось определить пользователя.");
        }

        try
        {
            var projects = await _context.Projects
                .Where(p => p.ManagerId == userId)
                .Include(p => p.Manager)
                .ToListAsync();

            var projectDTOs = projects.Select(project => new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Manager = project.Manager != null ? new UserDTO
                {
                    Id = project.Manager.Id,
                    Username = project.Manager.Username,
                    Email = project.Manager.Email
                } : null
            });

            return Ok(projectDTOs);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            // Предполагается, что у вас настроен ILogger<ProjectsController>
            // _logger.LogError(ex, "Ошибка при получении проектов пользователя с ID {UserId}.", userId);
            return StatusCode(500, "Произошла ошибка при получении проектов.");
        }
    }
}
