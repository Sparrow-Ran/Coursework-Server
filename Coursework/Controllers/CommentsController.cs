using Coursework.Models.DTOs;
using Coursework.Models;
using Coursework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class CommentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Создает новый комментарий.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> Create([FromBody] CreateCommentRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst("id").Value);

        var comment = new Comment
        {
            IssueId = model.IssueId,
            AuthorId = userId,
            Content = model.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var commentDTO = await GetCommentDTO(comment.Id);

        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, commentDTO);
    }


    /// <summary>
    /// Получает комментарии по идентификатору задачи.
    /// </summary>
    [HttpGet("GetByIssue/{issueId}")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetByIssue(int issueId)
    {
        var comments = await _context.Comments
            .Where(c => c.IssueId == issueId)
            .Include(c => c.Author)
            .Select(c => new CommentDTO
            {
                Id = c.Id,
                IssueId = c.IssueId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                Author = new UserDTO
                {
                    Id = c.Author.Id,
                    Username = c.Author.Username,
                    Email = c.Author.Email
                }
            })
            .ToListAsync();

        return Ok(comments);
    }

    /// <summary>
    /// Получает комментарий по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetById(int id)
    {
        var commentDTO = await GetCommentDTO(id);

        if (commentDTO == null)
        {
            return NotFound();
        }

        return Ok(commentDTO);
    }

    /// <summary>
    /// Удаляет комментарий.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirst("id").Value);
        if (comment.AuthorId != userId)
        {
            return Forbid();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<CommentDTO> GetCommentDTO(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return null;
        }

        var commentDTO = new CommentDTO
        {
            Id = comment.Id,
            IssueId = comment.IssueId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            Author = new UserDTO
            {
                Id = comment.Author.Id,
                Username = comment.Author.Username,
                Email = comment.Author.Email
            }
        };

        return commentDTO;
    }
}
