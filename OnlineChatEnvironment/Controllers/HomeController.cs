using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace OnlineChatEnvironment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var chats = db.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();

            return View(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room,
            };

            chat.Users.Add(new ChatUser
            {

                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Role = UserRole.Admin
            });

            db.Chats.Add(chat);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(Guid chatId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Role = UserRole.Member
            };

            db.ChatUsers.Add(chatUser);

            await db.SaveChangesAsync();

            return RedirectToAction("Chat", "Home", new { id = chatId });
        }

        public IActionResult Find()
        {
            var users = db.Users
                .Where(x => x.Id != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            List<Chat> chats = GetPrivateRooms();

            return View(chats);
        }

        private List<Chat> GetPrivateRooms()
        {
            var chats = db.Chats
                            .Include(x => x.Users)
                            .ThenInclude(x => x.User)
                            .Where(x => x.Type == ChatType.Private
                            && x.Users.Any(y => y.UserId == Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                            .ToList();
            return chats;
        }

        public async Task<IActionResult> CreatePrivateRoom(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            if (GetPrivateRooms().FirstOrDefault(x => x.Users.Any(y => y.UserId == userId)) is Chat exists)
            {
                return RedirectToAction("Chat", new { id = exists.Id }); ;
            }

            var chat = new Chat
            {
                Type = ChatType.Private,
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = currentUserId
            });

            db.Chats.Add(chat);
            await db.SaveChangesAsync();

            //return RedirectToAction("Chat", new Chat { Id = chat.Id});
            return RedirectToAction("Chat", new { id = chat.Id });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Chat(Guid id)
        {
            var chat = db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> JoinChat(Guid chatId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Role = UserRole.Member

            };

            db.ChatUsers.Add(chatUser);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Guid roomId, string message)
        {

            var messageText = new Message
            {
                ChatId = roomId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.UtcNow
            };

            db.Messages.Add(messageText);
            await db.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = roomId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}