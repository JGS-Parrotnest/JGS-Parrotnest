using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParrotnestServer.Data;
using Microsoft.AspNetCore.SignalR;
using ParrotnestServer.Hubs;
using ParrotnestServer.Models;
using ParrotnestServer.Services;
using System.Security.Claims;
namespace ParrotnestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserTracker _userTracker;
        private readonly IHubContext<ChatHub> _hubContext;

        public FriendsController(ApplicationDbContext context, IUserTracker userTracker, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userTracker = userTracker;
            _hubContext = hubContext;
        }
        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        private async Task<Friendship?> GetFriendshipAsync(int userId, int targetUserId)
        {
            return await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == userId && f.AddresseeId == targetUserId) ||
                (f.RequesterId == targetUserId && f.AddresseeId == userId));
        }

        private async Task<UserRelation?> GetRelationAsync(int requesterId, int targetUserId)
        {
            return await _context.UserRelations.FirstOrDefaultAsync(r =>
                r.RequesterId == requesterId && r.TargetUserId == targetUserId);
        }

        private static string BuildPrivateChatRestrictionReason(
            bool isFriend,
            bool isBlockedByMe,
            bool isBlockedByTarget,
            bool isIgnoredByMe,
            bool isIgnoredByTarget)
        {
            if (isBlockedByMe) return "Najpierw odblokuj tego użytkownika.";
            if (isBlockedByTarget) return "Ten użytkownik Cię zablokował.";
            if (isIgnoredByMe) return "Najpierw cofnij ignorowanie tego użytkownika.";
            if (isIgnoredByTarget) return "Ten użytkownik ignoruje Twoje wiadomości.";
            if (!isFriend) return "Wiadomości prywatne są dostępne tylko dla znajomych.";
            return string.Empty;
        }

        private async Task<object> BuildRelationStateAsync(int userId, User targetUser)
        {
            var friendship = await GetFriendshipAsync(userId, targetUser.Id);
            var myRelation = await GetRelationAsync(userId, targetUser.Id);
            var targetRelation = await GetRelationAsync(targetUser.Id, userId);

            var isFriend = friendship?.Status == FriendshipStatus.Accepted;
            var hasPendingOutgoingRequest = friendship?.Status == FriendshipStatus.Pending && friendship.RequesterId == userId;
            var hasPendingIncomingRequest = friendship?.Status == FriendshipStatus.Pending && friendship.AddresseeId == userId;
            var isBlockedByMe = myRelation?.RelationType == UserRelationType.Blocked;
            var isBlockedByTarget = targetRelation?.RelationType == UserRelationType.Blocked;
            var isIgnoredByMe = myRelation?.RelationType == UserRelationType.Ignored;
            var isIgnoredByTarget = targetRelation?.RelationType == UserRelationType.Ignored;
            var canSendPrivateMessage = isFriend && !isBlockedByMe && !isBlockedByTarget && !isIgnoredByMe && !isIgnoredByTarget;

            return new
            {
                targetUser = new
                {
                    targetUser.Id,
                    targetUser.Username,
                    targetUser.Email,
                    targetUser.AvatarUrl,
                    targetUser.Status
                },
                isFriend,
                hasPendingOutgoingRequest,
                hasPendingIncomingRequest,
                incomingRequestId = hasPendingIncomingRequest ? friendship?.Id : null,
                outgoingRequestId = hasPendingOutgoingRequest ? friendship?.Id : null,
                isBlockedByMe,
                isBlockedByTarget,
                isIgnoredByMe,
                isIgnoredByTarget,
                canSendPrivateMessage,
                chatDisabledReason = BuildPrivateChatRestrictionReason(
                    isFriend,
                    isBlockedByMe,
                    isBlockedByTarget,
                    isIgnoredByMe,
                    isIgnoredByTarget)
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFriends()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            var hiddenTargetIds = await _context.UserRelations
                .Where(r => r.RequesterId == userId.Value)
                .Select(r => r.TargetUserId)
                .ToListAsync();
            var friendships = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f => f.Status == FriendshipStatus.Accepted && 
                           (f.RequesterId == userId || f.AddresseeId == userId) &&
                           !hiddenTargetIds.Contains(f.RequesterId == userId ? f.AddresseeId : f.RequesterId))
                .Select(f => new
                {
                    Id = f.RequesterId == userId ? f.AddresseeId : f.RequesterId,
                    Username = f.RequesterId == userId ? (f.Addressee != null ? f.Addressee.Username : null) : (f.Requester != null ? f.Requester.Username : null),
                    Email = f.RequesterId == userId ? (f.Addressee != null ? f.Addressee.Email : null) : (f.Requester != null ? f.Requester.Email : null),
                    AvatarUrl = f.RequesterId == userId ? (f.Addressee != null ? f.Addressee.AvatarUrl : null) : (f.Requester != null ? f.Requester.AvatarUrl : null),
                    Status = f.RequesterId == userId ? (f.Addressee != null ? f.Addressee.Status : 1) : (f.Requester != null ? f.Requester.Status : 1)
                })
                .ToListAsync();
            var friendIds = friendships.Select(f => f.Id).ToList();
            var lastMessages = await _context.Messages
                .Where(m => (m.SenderId == userId && friendIds.Contains(m.ReceiverId ?? 0)) || 
                            (m.ReceiverId == userId && friendIds.Contains(m.SenderId)))
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => g.OrderByDescending(m => m.Timestamp).FirstOrDefault())
                .ToListAsync();
            var resultList = new List<object>();
            foreach(var f in friendships)
            {
                var lastMsg = lastMessages.FirstOrDefault(m => 
                    m != null &&
                    ((m.SenderId == userId && m.ReceiverId == f.Id) || 
                    (m.ReceiverId == userId && m.SenderId == f.Id)));
                var isOnline = await _userTracker.IsUserOnline(f.Id);
                int finalStatus = 0;
                if (isOnline) {
                    if (f.Status != 4) finalStatus = f.Status;
                }
                resultList.Add(new {
                    f.Id,
                    f.Username,
                    f.Email,
                    f.AvatarUrl,
                    LastMessage = lastMsg?.Content,
                    LastMessageTime = lastMsg?.Timestamp,
                    IsOnline = isOnline,
                    Status = finalStatus
                });
            }
            return Ok(resultList);
        }
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingRequests()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            var hiddenTargetIds = await _context.UserRelations
                .Where(r => r.RequesterId == userId.Value)
                .Select(r => r.TargetUserId)
                .ToListAsync();

            var pending = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f => f.Status == FriendshipStatus.Pending && f.AddresseeId == userId &&
                    !hiddenTargetIds.Contains(f.RequesterId))
                .Select(f => new
                {
                    Id = f.Id,
                    RequesterId = f.RequesterId,
                    Username = f.Requester != null ? f.Requester.Username : "Unknown",
                    Email = f.Requester != null ? f.Requester.Email : null,
                    AvatarUrl = f.Requester != null ? f.Requester.AvatarUrl : null,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(pending);
        }

        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<object>>> GetSentRequests()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            var hiddenTargetIds = await _context.UserRelations
                .Where(r => r.RequesterId == userId.Value)
                .Select(r => r.TargetUserId)
                .ToListAsync();

            var sent = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f => f.Status == FriendshipStatus.Pending && f.RequesterId == userId &&
                    !hiddenTargetIds.Contains(f.AddresseeId))
                .Select(f => new
                {
                    Id = f.Id,
                    AddresseeId = f.AddresseeId,
                    Username = f.Addressee != null ? f.Addressee.Username : "Unknown",
                    Email = f.Addressee != null ? f.Addressee.Email : null,
                    AvatarUrl = f.Addressee != null ? f.Addressee.AvatarUrl : null,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(sent);
        }

        [HttpGet("blocked")]
        public async Task<ActionResult<IEnumerable<object>>> GetBlockedUsers()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var blocked = await _context.UserRelations
                .Include(r => r.TargetUser)
                .Where(r => r.RequesterId == userId.Value && r.RelationType == UserRelationType.Blocked && r.TargetUser != null)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    Id = r.TargetUserId,
                    Username = r.TargetUser!.Username,
                    Email = r.TargetUser.Email,
                    AvatarUrl = r.TargetUser.AvatarUrl,
                    Status = r.TargetUser.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(blocked);
        }

        [HttpGet("ignored")]
        public async Task<ActionResult<IEnumerable<object>>> GetIgnoredUsers()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var ignored = await _context.UserRelations
                .Include(r => r.TargetUser)
                .Where(r => r.RequesterId == userId.Value && r.RelationType == UserRelationType.Ignored && r.TargetUser != null)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    Id = r.TargetUserId,
                    Username = r.TargetUser!.Username,
                    Email = r.TargetUser.Email,
                    AvatarUrl = r.TargetUser.AvatarUrl,
                    Status = r.TargetUser.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(ignored);
        }

        [HttpGet("mutual/{targetUserId:int}")]
        public async Task<IActionResult> GetMutualFriends(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            if (userId == targetUserId) return BadRequest("Cannot check mutual friends with yourself.");
            var myFriendIds = await _context.Friendships
                .Where(f => f.Status == FriendshipStatus.Accepted && 
                           (f.RequesterId == userId || f.AddresseeId == userId))
                .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();
            var targetFriendIds = await _context.Friendships
                .Where(f => f.Status == FriendshipStatus.Accepted && 
                           (f.RequesterId == targetUserId || f.AddresseeId == targetUserId))
                .Select(f => f.RequesterId == targetUserId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();
            var mutualIds = myFriendIds.Intersect(targetFriendIds).ToList();
            if (!mutualIds.Any()) return Ok(new List<object>());
            var mutualFriends = await _context.Users
                .Where(u => mutualIds.Contains(u.Id))
                .Select(u => new 
                {
                    u.Id,
                    u.Username,
                    u.AvatarUrl
                })
                .ToListAsync();
            return Ok(mutualFriends);
        }

        [HttpGet("relation/{targetUserId:int}")]
        public async Task<IActionResult> GetUserRelation(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            if (targetUserId == userId.Value) return BadRequest("Nie możesz sprawdzić relacji z samym sobą.");

            var targetUser = await _context.Users.FindAsync(targetUserId);
            if (targetUser == null) return NotFound("Użytkownik nie został znaleziony.");

            return Ok(await BuildRelationStateAsync(userId.Value, targetUser));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendDto dto)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            if (string.IsNullOrWhiteSpace(dto.UsernameOrEmail))
            {
                return BadRequest("Podaj nazwę użytkownika lub email.");
            }
            var search = dto.UsernameOrEmail.Trim();
            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == search || u.Email == search);
            if (targetUser == null)
            {
                return NotFound("Użytkownik nie został znaleziony.");
            }
            if (targetUser.Id == userId)
            {
                return BadRequest("Nie możesz dodać samego siebie.");
            }
            var myRelation = await GetRelationAsync(userId.Value, targetUser.Id);
            var theirRelation = await GetRelationAsync(targetUser.Id, userId.Value);
            if (myRelation?.RelationType == UserRelationType.Blocked)
            {
                return BadRequest("Najpierw odblokuj tego użytkownika.");
            }
            if (theirRelation?.RelationType == UserRelationType.Blocked)
            {
                return BadRequest("Nie możesz wysłać zaproszenia do tego użytkownika.");
            }
            if (myRelation?.RelationType == UserRelationType.Ignored)
            {
                return BadRequest("Najpierw cofnij ignorowanie tego użytkownika.");
            }
            if (theirRelation?.RelationType == UserRelationType.Ignored)
            {
                return BadRequest("Nie możesz wysłać zaproszenia do tego użytkownika.");
            }
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f => 
                    (f.RequesterId == userId && f.AddresseeId == targetUser.Id) ||
                    (f.RequesterId == targetUser.Id && f.AddresseeId == userId));
            if (existingFriendship != null)
            {
                if (existingFriendship.Status == FriendshipStatus.Accepted)
                {
                    return Ok(new { 
                        message = "Jesteście już znajomymi.", 
                        friendId = targetUser.Id,
                        username = targetUser.Username,
                        alreadyFriends = true
                    });
                }
                if (existingFriendship.Status == FriendshipStatus.Pending)
                {
                    if (existingFriendship.RequesterId == userId)
                    {
                        return BadRequest("Zaproszenie już zostało wysłane.");
                    }
                    else
                    {
                        existingFriendship.Status = FriendshipStatus.Accepted;
                        await _context.SaveChangesAsync();
                        var acceptingUser = await _context.Users.FindAsync(userId.Value);
                        if (acceptingUser != null)
                        {
                            await _hubContext.Clients.Group($"User_{targetUser.Id}").SendAsync("FriendRequestAccepted", new 
                            {
                                friendId = acceptingUser.Id,
                                username = acceptingUser.Username,
                                avatarUrl = acceptingUser.AvatarUrl
                            });
                        }

                        return Ok(new { 
                            message = "Zaproszenie zostało zaakceptowane.",
                            friendId = targetUser.Id,
                            username = targetUser.Username,
                            alreadyFriends = true
                        });
                    }
                }
            }
            var friendship = new Friendship
            {
                RequesterId = userId.Value,
                AddresseeId = targetUser.Id,
                Status = FriendshipStatus.Pending
            };
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
            var sender = await _context.Users.FindAsync(userId.Value);
            if (sender != null)
            {
                await _hubContext.Clients.Group($"User_{targetUser.Id}").SendAsync("FriendRequestReceived", new 
                {
                    requestId = friendship.Id,
                    senderId = sender.Id,
                    username = sender.Username,
                    avatarUrl = sender.AvatarUrl
                });
            }

            return Ok(new { 
                message = "Zaproszenie do znajomych zostało wysłane.",
                friendId = targetUser.Id,
                username = targetUser.Username,
                avatarUrl = targetUser.AvatarUrl,
                pending = true
            });
        }
        [HttpPost("accept/{friendshipId:int}")]
        public async Task<IActionResult> AcceptFriend(int friendshipId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == friendshipId && f.AddresseeId == userId);
            if (friendship == null)
            {
                return NotFound("Zaproszenie nie zostało znalezione.");
            }
            if (friendship.Status != FriendshipStatus.Pending)
            {
                return BadRequest("Zaproszenie nie jest w stanie oczekiwania.");
            }
            var myRelation = await GetRelationAsync(userId.Value, friendship.RequesterId);
            var theirRelation = await GetRelationAsync(friendship.RequesterId, userId.Value);
            if (myRelation != null || theirRelation != null)
            {
                return BadRequest("Nie możesz zaakceptować tego zaproszenia przy aktywnej blokadzie lub ignorowaniu.");
            }
            friendship.Status = FriendshipStatus.Accepted;
            await _context.SaveChangesAsync();
            var acceptingUser = await _context.Users.FindAsync(userId.Value);
            if (acceptingUser != null)
            {
                await _hubContext.Clients.Group($"User_{friendship.RequesterId}").SendAsync("FriendRequestAccepted", new 
                {
                    friendId = acceptingUser.Id,
                    username = acceptingUser.Username,
                    avatarUrl = acceptingUser.AvatarUrl
                });
            }

            return Ok(new { message = "Zaproszenie zostało zaakceptowane." });
        }

        [HttpPost("block/{targetUserId:int}")]
        public async Task<IActionResult> BlockUser(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            if (targetUserId == userId.Value) return BadRequest("Nie możesz zablokować samego siebie.");

            var targetUser = await _context.Users.FindAsync(targetUserId);
            if (targetUser == null) return NotFound("Użytkownik nie został znaleziony.");

            var relation = await GetRelationAsync(userId.Value, targetUserId);
            if (relation == null)
            {
                relation = new UserRelation
                {
                    RequesterId = userId.Value,
                    TargetUserId = targetUserId,
                    RelationType = UserRelationType.Blocked
                };
                _context.UserRelations.Add(relation);
            }
            else
            {
                relation.RelationType = UserRelationType.Blocked;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Użytkownik {targetUser.Username} został zablokowany." });
        }

        [HttpPost("unblock/{targetUserId:int}")]
        public async Task<IActionResult> UnblockUser(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var relation = await GetRelationAsync(userId.Value, targetUserId);
            if (relation == null || relation.RelationType != UserRelationType.Blocked)
            {
                return NotFound("Ten użytkownik nie jest zablokowany.");
            }

            _context.UserRelations.Remove(relation);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Użytkownik został odblokowany." });
        }

        [HttpPost("ignore/{targetUserId:int}")]
        public async Task<IActionResult> IgnoreUser(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            if (targetUserId == userId.Value) return BadRequest("Nie możesz ignorować samego siebie.");

            var targetUser = await _context.Users.FindAsync(targetUserId);
            if (targetUser == null) return NotFound("Użytkownik nie został znaleziony.");

            var relation = await GetRelationAsync(userId.Value, targetUserId);
            if (relation == null)
            {
                relation = new UserRelation
                {
                    RequesterId = userId.Value,
                    TargetUserId = targetUserId,
                    RelationType = UserRelationType.Ignored
                };
                _context.UserRelations.Add(relation);
            }
            else
            {
                relation.RelationType = UserRelationType.Ignored;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Użytkownik {targetUser.Username} został zignorowany." });
        }

        [HttpPost("unignore/{targetUserId:int}")]
        public async Task<IActionResult> UnignoreUser(int targetUserId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var relation = await GetRelationAsync(userId.Value, targetUserId);
            if (relation == null || relation.RelationType != UserRelationType.Ignored)
            {
                return NotFound("Ten użytkownik nie jest ignorowany.");
            }

            _context.UserRelations.Remove(relation);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Ignorowanie użytkownika zostało cofnięte." });
        }

        [HttpDelete("{friendId:int}")]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => 
                    (f.RequesterId == userId && f.AddresseeId == friendId) ||
                    (f.RequesterId == friendId && f.AddresseeId == userId));
            if (friendship == null)
            {
                friendship = await _context.Friendships.FindAsync(friendId);
                if (friendship == null || (friendship.RequesterId != userId && friendship.AddresseeId != userId))
                {
                    return NotFound("Znajomość nie została znaleziona.");
                }
            }
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usunięto." });
        }
    }
    public class AddFriendDto
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
    }
}
