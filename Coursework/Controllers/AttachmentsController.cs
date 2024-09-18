using Coursework.Models.DTOs;
using Coursework.Models;
using Coursework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class AttachmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AttachmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Создает новое вложение.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AttachmentDTO>> Create([FromBody] CreateAttachmentRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst("id").Value);

        var attachment = new Attachment
        {
            IssueId = model.IssueId,
            FilePath = model.FilePath,
            UploadedBy = userId,
            UploadedAt = DateTime.UtcNow
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        var attachmentDTO = await GetAttachmentDTO(attachment.Id);

        return CreatedAtAction(nameof(GetById), new { id = attachment.Id }, attachmentDTO);
    }

    /// <summary>
    /// Получает вложение по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AttachmentDTO>> GetById(int id)
    {
        var attachmentDTO = await GetAttachmentDTO(id);

        if (attachmentDTO == null)
        {
            return NotFound();
        }

        return Ok(attachmentDTO);
    }

    /// <summary>
    /// Удаляет вложение.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var attachment = await _context.Attachments.FindAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirst("id").Value);
        if (attachment.UploadedBy != userId)
        {
            return Forbid();
        }

        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<AttachmentDTO> GetAttachmentDTO(int id)
    {
        var attachment = await _context.Attachments
            .Include(a => a.Uploader)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attachment == null)
        {
            return null;
        }

        var attachmentDTO = new AttachmentDTO
        {
            Id = attachment.Id,
            IssueId = attachment.IssueId,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            Uploader = new UserDTO
            {
                Id = attachment.Uploader.Id,
                Username = attachment.Uploader.Username,
                Email = attachment.Uploader.Email
            }
        };

        return attachmentDTO;
    }
}
