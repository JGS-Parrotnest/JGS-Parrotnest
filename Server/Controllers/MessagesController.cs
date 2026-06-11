using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ParrotnestServer.Data;
using ParrotnestServer.Hubs;
using ParrotnestServer.Models;
using System.Security.Claims;
using Message = ParrotnestServer.Models.Message;
namespace ParrotnestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ChatHub> _hubContext;
        public MessagesController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
            _hubContext = hubContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMessages([FromQuery] int? receiverId = null, [FromQuery] int? groupId = null)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                IQueryable<Message> query = _context.Messages.Include(m => m.Sender).Include(m => m.Receiver);
                if (groupId.HasValue)
                {
                    var isMember = await _context.GroupMembers.AnyAsync(gm => gm.GroupId == groupId.Value && gm.UserId == userId);
                    if (!isMember) return Unauthorized("Nie jesteś członkiem tej grupy.");
                    query = query.Where(m => m.GroupId == groupId.Value);
                }
                else if (receiverId.HasValue)
                {
                    query = query.Where(m => 
                        m.GroupId == null && 
                        m.Sender != null &&
                        ((m.SenderId == userId && m.ReceiverId == receiverId) ||
                         (m.SenderId == receiverId && m.ReceiverId == userId)));
                }
                else
                {
                    query = query.Where(m => m.GroupId == null && m.ReceiverId == null && m.Sender != null);
                }
                var messages = await query
                    .OrderBy(m => m.Timestamp)
                    .Select(m => new 
                    {
                        Id = m.Id,
                        Content = m.Content ?? string.Empty,
                        Sender = m.Sender != null ? m.Sender.Username : "Nieznany",
                        SenderId = m.SenderId,
                        SenderAvatarUrl = m.Sender != null ? m.Sender.AvatarUrl : null,
                        ReceiverId = m.ReceiverId,
                        Timestamp = m.Timestamp,
                        ImageUrl = m.ImageUrl,
                        ReplyToId = m.ReplyToId,
                        ReplyToSender = m.ReplyTo != null && m.ReplyTo.Sender != null ? m.ReplyTo.Sender.Username : null,
                        ReplyToContent = m.ReplyTo != null ? m.ReplyTo.Content : null,
                        Reactions = m.Reactions
                    })
                    .ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Błąd podczas pobierania wiadomości", message = ex.Message });
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nie wybrano pliku.");

            var clientPath = _configuration["ClientPath"] ?? Path.Combine(_environment.ContentRootPath, "..", "Client");
            var uploadsFolder = Path.Combine(clientPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return Ok(new { url = fileUrl });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized("Brak identyfikatora użytkownika (Claim mismatch).");
            var userId = int.Parse(userIdStr);

            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound("Wiadomość nie znaleziona.");
            var requester = await _context.Users.FindAsync(userId);
            var isAdmin = requester != null && requester.IsAdmin;
            if (message.SenderId != userId && !isAdmin) {
                return StatusCode(403, $"Możesz usuwać tylko własne wiadomości.");
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Wiadomość usunięta." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] UpdateMessageDto dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized("Brak identyfikatora użytkownika.");
            if (dto == null) return BadRequest(new { message = "Brak danych do zapisania." });

            var userId = int.Parse(userIdStr);
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound(new { message = "Wiadomość nie znaleziona." });
            if (message.SenderId != userId) return StatusCode(403, new { message = "Możesz edytować tylko własne wiadomości." });

            var normalizedContent = (dto.Content ?? string.Empty).Replace("\r\n", "\n").TrimEnd();
            if (string.IsNullOrWhiteSpace(normalizedContent) && string.IsNullOrWhiteSpace(message.ImageUrl))
            {
                return BadRequest(new { message = "Wiadomość nie może być pusta." });
            }

            message.Content = normalizedContent;
            await _context.SaveChangesAsync();

            var payload = new
            {
                id = message.Id,
                content = message.Content ?? string.Empty
            };

            if (message.GroupId.HasValue)
            {
                await _hubContext.Clients.Group($"Group_{message.GroupId.Value}").SendAsync("MessageEdited", payload);
            }
            else if (message.ReceiverId.HasValue)
            {
                await _hubContext.Clients.Group($"User_{message.SenderId}").SendAsync("MessageEdited", payload);
                await _hubContext.Clients.Group($"User_{message.ReceiverId.Value}").SendAsync("MessageEdited", payload);
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("MessageEdited", payload);
            }

            return Ok(payload);
        }

        public class UpdateMessageDto
        {
            public string? Content { get; set; }
        }
    }
}
