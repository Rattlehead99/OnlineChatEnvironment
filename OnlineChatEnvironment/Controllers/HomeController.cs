using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Models;
using System.Diagnostics;
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
                .Where(x => !x.Users.Any(y => y.UserId == userId))
                .ToList();

            return View(chats);
        }

        public IActionResult Privacy()
        {
            return View();
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

        [HttpGet("{id}")]
        public IActionResult Chat(Guid id)
        {
            var chat = db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Guid chatId, string message)
        {

            var messageText = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.UtcNow
            };

            db.Messages.Add(messageText);
            await db.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId});
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

            return RedirectToAction("Chat", "Home", new {id = chatId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}